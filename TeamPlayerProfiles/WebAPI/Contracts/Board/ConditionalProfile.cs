using Library.Models.QueryConditions;

namespace WebAPI.Contracts.Board
{
    public static class ConditionalProfile
    {
        public abstract class Request
        {
            public StringFilter? NameFilter { get; set; }

            public StringFilter? DescriptionFilter { get; set; }

            public SingleValueFilter<bool?>? DisplayedFilter { get; set; }

            public DateTimeFilter? UpdatedAtStart { get; set; }

            public DateTimeFilter? UpdatedAtEnd { get; set; }

            public SortCondition? Sort { get; set; }
        }

        public class PlayerRequest : Request
        {
            public ValueListFilter<int?>? PositionFilter { get; set; }

            public ValueListFilter<int>? HeroFilter { get; set; }
        }

        public class TeamRequest : Request
        {
            public NumericFilter<int>? PlayerCountStart { get; set; }

            public NumericFilter<int>? PlayerCountEnd { get; set; }

            public ValueListFilter<int>? HeroFilter { get; set; }

            public ValueListFilter<int>? PositionFilter { get; set; }
        }
    }
}
