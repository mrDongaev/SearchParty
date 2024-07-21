using Common.Models.Enums;

namespace WebAPI.Contracts.Hero
{
    public static class GetHero
    {
        public sealed class Response
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public MainStat MainStat { get; set; }
        }
    }
}
