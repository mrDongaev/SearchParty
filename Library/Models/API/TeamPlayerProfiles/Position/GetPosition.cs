using Library.Models.Enums;

namespace Library.Models.API.TeamPlayerProfiles.Position
{
    public static class GetPosition
    {
        public sealed class Response
        {
            public int Id { get; set; }

            public PositionName Name { get; set; }
        }
    }
}
