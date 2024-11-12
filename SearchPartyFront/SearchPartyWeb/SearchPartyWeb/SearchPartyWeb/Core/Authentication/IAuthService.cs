namespace SearchPartyWeb.Core.Authentication;

public interface IAuthService
{
    Task Login(string email, string password);
}