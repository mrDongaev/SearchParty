using Library.Models.QueryConditions;

namespace Common.Models
{
    public class ConditionalTeamQuery : ConditionalProfileQuery
    {
        public NumericFilter<int>? PlayerCountStart { get; set; }

        public NumericFilter<int>? PlayerCountEnd { get; set; }

        public ValueListFilter<int>? HeroFilter { get; set; }

        public ValueListFilter<int>? PositionFilter { get; set; }
    }
}
