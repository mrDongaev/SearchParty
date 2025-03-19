using Library.Models.QueryConditions;

namespace Common.Models
{
    public abstract class ConditionalProfileQuery
    {
        public StringFilter? NameFilter { get; set; }

        public StringFilter? DescriptionFilter { get; set; }

        public DateTimeFilter? UpdatedAtFilter { get; set; }

        public DateTimeFilter? UpdatedAtAddFilter { get; set; }

        public NumericFilter<uint> MmrFilter { get; set; }

        public NumericFilter<uint> MmrAddFilter { get; set; }

        public ICollection<SortCondition>? SortConditions { get; set; }
    }
}
