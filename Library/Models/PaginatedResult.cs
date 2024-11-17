namespace Library.Models
{
    public class PaginatedResult<T>
    {
        public int Total { get; set; } = 0;

        public int Page { get; set; } = 0;

        public int PageSize { get; set; } = 10;

        public int PageCount { get; set; } = 0;

        public ICollection<T> List { get; set; } = [];
    }
}
