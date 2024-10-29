using Library.Models.QueryConditions;
using WebAPI.Contracts.Board;

namespace WebAPI.Contracts.Player
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
