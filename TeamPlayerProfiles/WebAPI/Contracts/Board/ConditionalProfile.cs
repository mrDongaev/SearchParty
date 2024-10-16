using static Common.Models.ConditionalQuery;
using static Common.Models.ConditionalQuery.Filter;

namespace WebAPI.Contracts.Board
{
    public static class ConditionalProfile
    {
        public abstract class Request
        {
            public StringFilter? NameFilter { get; set; }

            public StringFilter? DescriptionFilter { get; set; }

            public SingleValueFilter<bool?>? DisplayedFilter { get; set; }

            public TimeFilter? UpdatedAtStart { get; set; }

            public TimeFilter? UpdatedAtEnd { get; set; }

            public Sort? Sort { get; set; }
        }

        public class PlayerRequest : Request
        {
            public ValueFilter<int?>? PositionFilter { get; set; }

            public ValueFilter<int>? HeroFilter { get; set; }
        }

        public class TeamRequest : Request
        {
            public NumericFilter<int>? PlayerCountStart { get; set; }

            public NumericFilter<int>? PlayerCountEnd { get; set; }

            public ValueFilter<int>? HeroFilter { get; set; }

            public ValueFilter<int>? PositionFilter { get; set; }
        }
    }
}
