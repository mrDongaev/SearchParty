using System.Text.Json;
using SearchPartyWeb.Core.ApiExecutor;

namespace SearchPartyWeb.Core.Authentication;

public class AuthService : IAuthService
{
    public IWebApiExecutor _webApiExecutor;

    public AuthService(WebApiExecutor webApiExecutor)
    {
        _webApiExecutor = webApiExecutor;
    }

    public async Task Login(string email, string password)
    {
        string data =JsonSerializer.Serialize(new AutentificationRequestMessage(email,password));
        var response = await _webApiExecutor.InvokePost($"api/User/login", data);
        if (String.IsNullOrWhiteSpace(response))
        {
           var result =  JsonSerializer.Deserialize<LoginResponse>(response);
        }
    }
}