using System.Text.Json.Serialization;

namespace SearchPartyWeb.Core.Models;

public class Invantion
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("sendingUserId")]
    public Guid SendingUserId { get; set; }

    [JsonPropertyName("acceptingUserId")]
    public Guid AcceptingUserId { get; set; }

    [JsonPropertyName("positionName")]
    public string PositionName { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("issuedAt")]
    public DateTimeOffset IssuedAt { get; set; }

    [JsonPropertyName("expiresAt")]
    public DateTimeOffset ExpiresAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("invitingTeamId")]
    public Guid InvitingTeamId { get; set; }

    [JsonPropertyName("acceptingPlayerId")]
    public Guid AcceptingPlayerId { get; set; }
}