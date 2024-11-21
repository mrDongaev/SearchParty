namespace SearchPartyWeb.Core.Authentication;

public interface IAuthService
{
    Task<LoginResponse> Login(string email, string password);
}