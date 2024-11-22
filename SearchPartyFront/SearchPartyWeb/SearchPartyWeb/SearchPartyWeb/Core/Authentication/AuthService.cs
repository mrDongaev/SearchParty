using System.Text.Json;
using SearchPartyWeb.Core.ApiExecutor;
using SearchPartyWeb.Core.Models;

namespace SearchPartyWeb.Core.Authentication;

public class AuthService : IAuthService
{
    public IWebApiExecutor _webApiExecutor;

    public AuthService(WebApiExecutor webApiExecutor)
    {
        _webApiExecutor = webApiExecutor;
    }

    public async Task<LoginResponse> Login(string email, string password)
    {
        
        var response = await _webApiExecutor.InvokePost<LoginResponse,AutentificationRequestMessage >(
            $"api/User/login", 
            new AutentificationRequestMessage(email, password)
            );
        if (response!=null)
        {
            return response;
        }

        return null;
    }
    
    
    
    public async Task<LoginResponse> Registrate(UserRegistrationModel user)
    {
        
        var response = await _webApiExecutor.InvokePost<LoginResponse,UserRegistrationModel >(
            $"api/User/registration", user);
        if (response!=null)
        {
            return response;
        }

        return null;
    }
    
}