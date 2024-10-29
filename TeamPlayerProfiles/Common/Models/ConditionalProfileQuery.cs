using Library.Models.QueryConditions;

namespace Common.Models
{
    public abstract class ConditionalProfileQuery
    {
        public StringFilter? NameFilter { get; set; }

        public StringFilter? DescriptionFilter { get; set; }

        public SingleValueFilter<bool?>? DisplayedFilter { get; set; }

        public DateTimeFilter? UpdatedAtStart { get; set; }

        public DateTimeFilter? UpdatedAtEnd { get; set; }

        public SortCondition? Sort { get; set; }
    }
}
