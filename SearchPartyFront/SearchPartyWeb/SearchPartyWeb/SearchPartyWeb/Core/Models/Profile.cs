using System.Text.Json.Serialization;

namespace SearchPartyWeb.Core.Models;

public class Profile
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("mmr")]
    public long Mmr { get; set; }

    [JsonPropertyName("displayed")]
    public bool Displayed { get; set; }

    [JsonPropertyName("position")]
    public string Position { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("heroes")]
    public List<Hero> Heroes { get; set; }
}