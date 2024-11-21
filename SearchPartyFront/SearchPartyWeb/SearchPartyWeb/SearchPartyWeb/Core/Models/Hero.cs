using System.Text.Json.Serialization;

namespace SearchPartyWeb.Core.Models;

public partial class Hero
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("mainStat")]
    public string MainStat { get; set; }
}