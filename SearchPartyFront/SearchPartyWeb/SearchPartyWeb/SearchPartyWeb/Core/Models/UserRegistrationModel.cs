using System.Text.Json.Serialization;

namespace SearchPartyWeb.Core.Models;

public class UserRegistrationModel
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")] 
    public string Password { get; set; }

    [JsonPropertyName("email")] 
    public string Email { get; set; }
}