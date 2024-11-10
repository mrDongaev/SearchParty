using Library.Models.API.Authentication.Token;

namespace Library.Services.Interfaces.AuthenticationInterfaces
{
    public interface IAuthenticationService
    {
        Task<RefreshToken.Response?> RefreshAccessToken(string refreshToken, CancellationToken cancellationToken);
    }
}
