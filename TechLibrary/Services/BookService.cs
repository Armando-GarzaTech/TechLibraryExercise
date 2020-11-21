using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Data;
using TechLibrary.Domain;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookid);
        Task<(int RecordCount, IEnumerable<Book> Books)> GetBooksPageAsync(int page, int pageSize, string filter);
        Task UpdateBookAsync(Book book);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _dataContext.Books.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        public async Task<(int RecordCount, IEnumerable<Book> Books)> GetBooksPageAsync(int page, int pageSize, string filter)
        {
            IQueryable<Book> books = _dataContext.Books;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var searchText = filter.ToLower();
                books = books.Where(b => b.Title.ToLower().Contains(searchText) || b.ShortDescr.ToLower().Contains(searchText));
            }

            return
            (
                RecordCount: await books.CountAsync(),
                Books: await books.Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync()
            );
        }

        public async Task UpdateBookAsync(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new ArgumentException("Book title required", nameof(book.Title));
            }

            var record = await _dataContext.Books.FirstAsync(b => b.BookId == book.BookId);
            record.ShortDescr = book.ShortDescr;
            record.Title = book.Title;
            await _dataContext.SaveChangesAsync();
        }
    }
}
