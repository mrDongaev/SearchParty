using System.Text.Json.Serialization;

namespace SearchPartyWeb.Core.Models;

public class CreateTeamModel
{
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
    public List<CreateTeamPlayersInTeamModel> PlayersInTeam { get; set; }
}

public class CreateTeamPlayersInTeamModel
{
    [JsonPropertyName("position")]
    public string Position { get; set; }

    [JsonPropertyName("playerId")]
    public string PlayerId { get; set; }
}

