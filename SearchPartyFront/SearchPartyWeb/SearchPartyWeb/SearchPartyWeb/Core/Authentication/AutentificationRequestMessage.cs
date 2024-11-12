using System.Text.Json.Serialization;

namespace SearchPartyWeb.Core.Authentication;

public class AutentificationRequestMessage
{
    public AutentificationRequestMessage(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
    
}