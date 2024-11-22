using System.Text.Json.Serialization;

namespace SearchPartyWeb.Core.Models;

public class Team
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("avgMmr")]
    public long AvgMmr { get; set; }

    [JsonPropertyName("displayed")]
    public bool Displayed { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("playersInTeam")]
    public List<PlayersInTeam> PlayersInTeam { get; set; }
}