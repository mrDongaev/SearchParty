using Library.Models.API.TeamPlayerProfiles.Board;
using Library.Models.QueryConditions;

namespace Library.Models.API.TeamPlayerProfiles.Player
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
