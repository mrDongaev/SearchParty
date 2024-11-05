using Library.Models.Enums;

namespace Library.Models.API.TeamPlayerProfiles.Hero
{
    public static class GetHero
    {
        public sealed class Response
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public MainStat MainStat { get; set; }
        }
    }
}
