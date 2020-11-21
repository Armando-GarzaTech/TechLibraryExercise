using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Services;

namespace TechLibrary.Test.Services
{
    [TestFixture()]
    [Category("BookService Search Tests")]
    public class BookServiceSearchTests
    {
        public DataContext _dataContext;
        private BookService _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "SearchDatabase")
                .Options;

            _dataContext = new DataContext(options);
            _sut = new BookService(_dataContext);

            CreateBooks(count: 100);
        }

        private void CreateBooks(int count)
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

        [Test]
        public async Task Find_book_by_title_not_found()
        {
            // Arrange 
            var bookTitle = "Pandora";

            // Act
            var result = await _sut.GetBooksPageAsync(page: 1, pageSize: 10, filter: bookTitle);

            // Assert
            Assert.That(result.Books.Count() == 0);
        }


        [Test]
        public async Task Find_book_by_title()
        {
            // Arrange 
            var bookTitle = "Book Title 42";

            // Act
            var result = await _sut.GetBooksPageAsync(page: 1, pageSize: 10, filter: bookTitle);

            // Assert
            Assert.That(result.Books.Any(b => b.Title == bookTitle));
        }

        [Test]
        public async Task Find_books_by_title_that_contain_search_text()
        {
            // Arrange 
            var bookTitle = "Title 4";

            // Act
            var result = await _sut.GetBooksPageAsync(page: 1, pageSize: 10, filter: bookTitle);

            // Assert
            Assert.That(result.Books.Count() == 10);
        }

        [Test]
        public async Task Find_book_by_description()
        {
            // Arrange 
            var bookDesc = "ShortDesc42";

            // Act
            var result = await _sut.GetBooksPageAsync(page: 1, pageSize: 10, filter: bookDesc);

            // Assert
            Assert.That(result.Books.Any(b => b.ShortDescr == bookDesc));
        }

        [Test]
        public async Task Find_books_by_description_that_contain_search_text()
        {
            // Arrange 
            var bookDesc = "Desc4";

            // Act
            var result = await _sut.GetBooksPageAsync(page: 1, pageSize: 10, filter: bookDesc);

            // Assert
            Assert.That(result.Books.Count() == 10);
        }

    }
}
