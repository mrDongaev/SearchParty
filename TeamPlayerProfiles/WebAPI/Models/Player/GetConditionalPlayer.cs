using Library.Models.QueryConditions;
using WebAPI.Models.Board;

namespace WebAPI.Models.Player
{
    public static class GetConditionalPlayer
    {
        public sealed class Request : GetConditionalProfile.Request
        {
            public ValueListFilter<int?>? PositionFilter { get; set; }

            public ValueListFilter<int>? HeroFilter { get; set; }
        }
    }
}
