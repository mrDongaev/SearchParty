using Library.Models.QueryConditions;

namespace Common.Models
{
    public static class ConditionalProfileQuery
    {
        public abstract class ProfileConditions
        {
            public StringFilter? NameFilter { get; set; }

            public StringFilter? DescriptionFilter { get; set; }

            public SingleValueFilter<bool?>? DisplayedFilter { get; set; }

            public DateTimeFilter? UpdatedAtStart { get; set; }

            public DateTimeFilter? UpdatedAtEnd { get; set; }

            public SortCondition? Sort { get; set; }
        }

        public sealed class TeamConditions : ProfileConditions
        {
            public NumericFilter<int>? PlayerCountStart { get; set; }

            public NumericFilter<int>? PlayerCountEnd { get; set; }

            public ValueListFilter<int>? HeroFilter { get; set; }

            public ValueListFilter<int>? PositionFilter { get; set; }
        }

        public sealed class PlayerConditions : ProfileConditions
        {
            public ValueListFilter<int?>? PositionFilter { get; set; }

            public ValueListFilter<int>? HeroFilter { get; set; }
        }
    }
}
