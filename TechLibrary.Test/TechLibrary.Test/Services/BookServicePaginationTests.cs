using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Services;
using System;
using System.Linq;

namespace TechLibrary.Test.Services
{
    [TestFixture()]
    [Category("BookService Pagination Tests")]
    public class BookServicePaginationTests
    {
        public DataContext _dataContext;
        private BookService _sut;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "PaginationDatabase")
                .Options;

            _dataContext = new DataContext(options);
            _sut = new BookService(_dataContext);
        }

        [TearDown]
        public void Teardown()
        {
            var books = _dataContext.Books.ToList();
            _dataContext.Books.RemoveRange(books);
            _dataContext.SaveChanges();
        }

        private void CreateBooks(int count = 1)
        {
            int id = 0;
            for (int i = 0; i<count; i++)
            {
                id++;
                var book = new Book
                {
                    BookId = id,
                    ISBN = $"1234-{id}",
                    LongDescr = $"LongDesc{id}",
                    PublishedDate = DateTime.Now.ToString(),
                    ShortDescr = $"ShortDesc{id}",
                    ThumbnailUrl = "",
                    Title = $"Book Title {id}"
                };
                _dataContext.Add(book);
            }
            _dataContext.SaveChanges();
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public async Task Pagination_Alsways_Return_First_Page(int page)
        {
            //Arrange
            CreateBooks(count: 10);
            var firstBook = _dataContext.Books.First();

            //Act
            var result = await _sut.GetBooksPageAsync(page, pageSize: 10, filter: "");

            //Assert
            Assert.That(result.Books.First().BookId == firstBook.BookId);
        }

        [Test]
        public async Task Pagination_Returns_Last_Page()
        {
            //Arrange
            CreateBooks(count: 100);

            //Act
            var result = await _sut.GetBooksPageAsync(page: 10, pageSize: 10, filter: "");

            //Assert
            Assert.That(result.RecordCount == 100 && result.Books.Count() == 10);
        }

        [Test]
        public async Task Pagination_Returns_Partial_Page()
        {
            //Arrange
            CreateBooks(count: 105);

            //Act
            var result = await _sut.GetBooksPageAsync(page: 11, pageSize: 10, filter: "");

            //Assert
            Assert.That(result.RecordCount == 105 && result.Books.Count() == 5);
        }

        [Test]
        public async Task Pagination_Returns_Empty_Page()
        {
            //Arrange
            CreateBooks(count: 100);

            //Act
            var result = await _sut.GetBooksPageAsync(page: 11, pageSize: 10, filter: "") ;

            //Assert
            Assert.That(result.RecordCount == 100 && result.Books.Count() == 0);
        }
    }
}
