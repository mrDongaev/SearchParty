using System.Text.Json;
using Blazored.LocalStorage;
using SearchPartyWeb.Core.Authentication;

namespace SearchPartyWeb.Core.Utils;

public static class BearerToken
{
    private static ILocalStorageService _localStorage;

    public static ILocalStorageService localStorage
    {
        get
        {
            return _localStorage;
        }
    }
    
    public static async Task<string> GetToken()
    {
        var s = await _localStorage.GetItemAsStringAsync("Authentication");
        var token = JsonSerializer.Deserialize<LoginResponse>(s);
        return token.AccessToken;
    }
}