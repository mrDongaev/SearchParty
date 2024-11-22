using SearchPartyWeb.Core.Models;

namespace SearchPartyWeb.Core.Authentication;

public interface IAuthService
{
    Task<LoginResponse> Login(string email, string password);
    Task<LoginResponse> Registrate(UserRegistrationModel user);
}