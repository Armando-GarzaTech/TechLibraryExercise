namespace TechLibrary.Contracts.Requests
{
    public class UpdateBookRequest
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
    }
}
