using Library.Models.Enums;

namespace WebAPI.Models.Position
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
