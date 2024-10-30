using Library.Models.QueryConditions;

namespace Common.Models
{
    public class ConditionalUserQuery
    {
        public NumericFilter<uint>? MinMmr { get; set; }

        public NumericFilter<uint>? MaxMmr { get; set; }

        public SortCondition Sort { get; set; }
    }
}
