using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Services;
using TechLibrary.Contracts.Requests;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get all books");

            var books = await _bookService.GetBooksAsync();

            var bookResponse = _mapper.Map<List<BookResponse>>(books);

            return Ok(bookResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }

        [HttpPost()]
        public async Task<ActionResult<BookResponse>> FetchBooks(BookPageRequest data)
        {
            _logger.LogInformation($"Get {data.PerPage} books");

            var result = await _bookService.GetBooksPageAsync(data.CurrentPage, data.PerPage, data.Filter);

            var response = new BookListResponse
            {
                RecordCount = result.RecordCount,
                Books = _mapper.Map<List<BookResponse>>(_mapper.Map<List<BookResponse>>(result.Books)),
            };

            return Ok(response);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateBook(UpdateBookRequest data)
        {
            _logger.LogInformation($"Update Book");
            var book = _mapper.Map<Book>(data);

            await _bookService.UpdateBookAsync(book);
            return Ok();
        }
    }
}
