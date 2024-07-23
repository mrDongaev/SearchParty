using Common.Models.Enums;
using System.Text.Json.Serialization;

namespace WebAPI.Contracts.Hero
{
    public static class GetHero
    {
        public sealed class Response
        {
            public int Id { get; set; }

            public string Name { get; set; }

            [JsonConverter(typeof(JsonStringEnumConverter))]
            public MainStat MainStat { get; set; }
        }
    }
}
