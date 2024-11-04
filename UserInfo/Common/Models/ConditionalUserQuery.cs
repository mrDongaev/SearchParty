using Library.Models.QueryConditions;

namespace Common.Models
{
    public class ConditionalUserQuery
    {
        public ValueListFilter<Guid> UserIds { get; set; }

        public NumericFilter<uint>? MinMmr { get; set; }

        public NumericFilter<uint>? MaxMmr { get; set; }

        public ICollection<SortCondition>? SortCondition { get; set; }
    }
}
