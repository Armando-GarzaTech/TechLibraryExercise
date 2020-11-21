using System.Collections.Generic;

namespace TechLibrary.Models
{
    public class BookListResponse
    {
        public int RecordCount { get; set; }
        public List<BookResponse> Books { get; set; }

    }
}
