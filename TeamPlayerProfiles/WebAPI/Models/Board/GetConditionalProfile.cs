using Library.Models.QueryConditions;

namespace WebAPI.Models.Board
{
    public static class GetConditionalProfile
    {
        public abstract class Request
        {
            public StringFilter? NameFilter { get; set; }

            public StringFilter? DescriptionFilter { get; set; }

            public SingleValueFilter<bool?>? DisplayedFilter { get; set; }

            public DateTimeFilter? UpdatedAtStart { get; set; }

            public DateTimeFilter? UpdatedAtEnd { get; set; }

            public ICollection<SortCondition>? SortConditions { get; set; }
        }
    }
}
