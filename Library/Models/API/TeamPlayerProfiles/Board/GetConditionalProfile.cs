using Library.Models.QueryConditions;

namespace Library.Models.API.TeamPlayerProfiles.Board
{
    public static class GetConditionalProfile
    {
        public abstract class Request
        {
            public StringFilter? NameFilter { get; set; }

            public StringFilter? DescriptionFilter { get; set; }

            public DateTimeFilter? UpdatedAtFilter { get; set; }

            public DateTimeFilter? UpdatedAtAddFilter { get; set; }

            public NumericFilter<uint>? MmrFilter { get; set; }

            public NumericFilter<uint>? MmrAddFilter { get; set; }

            public ICollection<SortCondition>? SortConditions { get; set; }
        }
    }
}
