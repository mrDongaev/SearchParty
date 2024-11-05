using Library.Models.QueryConditions;
using WebAPI.Models.Board;

namespace WebAPI.Models.Team
{
    public static class GetConditionalTeam
    {
        public sealed class Request : GetConditionalProfile.Request
        {
            public NumericFilter<int>? PlayerCountStart { get; set; }

            public NumericFilter<int>? PlayerCountEnd { get; set; }

            public ValueListFilter<int>? HeroFilter { get; set; }

            public ValueListFilter<int>? PositionFilter { get; set; }
        }
    }
}
