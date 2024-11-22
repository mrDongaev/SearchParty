using System.Text.Json.Serialization;

namespace SearchPartyWeb.Core.Models;

public class PlayersInTeam
{
    [JsonPropertyName("position")]
    public string Position { get; set; }

    [JsonPropertyName("player")]
    public Player Player { get; set; }
}
