namespace TechLibrary.Contracts.Requests
{
    public class BookPageRequest
    {
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public string Filter { get; set; }
    }
}
