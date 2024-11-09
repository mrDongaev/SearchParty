using Library.Models.API.Authentication.Token;
using Library.Services.Interfaces.AuthenticationInterfaces;

namespace Library.Services.Implementations.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RefreshToken.Response?> RefreshAccessToken(string refreshToken, CancellationToken cancellationToken)
        {
            RefreshToken.Request request = new RefreshToken.Request()
            {
                RefreshToken = refreshToken,
            };
            using var response = await _httpClient.PostAsJsonAsync("/api/Token/refreshtoken", request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RefreshToken.Response>(cancellationToken);
            }
            else
            {
                return null;
            }
        }
    }
}
