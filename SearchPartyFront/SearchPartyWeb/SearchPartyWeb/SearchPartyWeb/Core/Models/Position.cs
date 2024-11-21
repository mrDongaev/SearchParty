using System.Text.Json.Serialization;

namespace SearchPartyWeb.Core.Models;

public class Position
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}