using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Services;

namespace TechLibrary.Test.Services
{
    [TestFixture()]
    [Category("BookService Editing Tests")]
    public class BookServiceEditingTests
    {
        public DataContext _dataContext;
        private BookService _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "EditingDatabase")
                .Options;

            _dataContext = new DataContext(options);
            _sut = new BookService(_dataContext);

            CreateBooks(count: 100);
        }

        private void CreateBooks(int count)
        {
            int id = 0;
            for (int i = 0; i < count; i++)
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
        public async Task Save_empty_book_title_thows_exception()
        {
            // Arrange 
            var newTitle = "";
            var book = await _sut.GetBookByIdAsync(1);

            // Act
            book.Title = newTitle;

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _sut.UpdateBookAsync(book));
        }


        [Test]
        public async Task Save_changes_to_book_title()
        {
            // Arrange 
            var newTitle = "New Book Title";
            var book = await _sut.GetBookByIdAsync(1);
            book.Title = newTitle;

            // Act
            await _sut.UpdateBookAsync(book);
            var updatedBook = await _sut.GetBookByIdAsync(1);

            // Assert
            Assert.That(updatedBook.Title == newTitle);
        }

        [Test]
        public async Task Save_changes_to_book_description()
        {
            // Arrange 
            var newDescription = "New Book Description";
            var book = await _sut.GetBookByIdAsync(1);
            book.ShortDescr = newDescription;

            // Act
            await _sut.UpdateBookAsync(book);
            var updatedBook = await _sut.GetBookByIdAsync(1);

            // Assert
            Assert.That(updatedBook.ShortDescr == newDescription);
        }
    }
}
