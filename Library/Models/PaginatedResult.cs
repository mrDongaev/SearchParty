namespace Library.Models
{
    public class PaginatedResult<T>
    {
        public uint Total { get; set; } = 0;

        public uint Page { get; set; } = 1;

        public uint PageSize { get; set; } = 10;

        public uint PageCount { get; set; } = 1;

        public ICollection<T> List { get; set; } = [];
    }
}
