using Library.Models.API.Authentication.Token;
using Library.Services.Interfaces.AuthenticationInterfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Library.Middleware
{
    public class TokenRefreshMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenRefreshMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthenticationService authService)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                await _next(context);
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                var refreshToken = context.Request.Cookies["RefreshToken"];

                if (refreshToken != null)
                {
                    RefreshToken.Response? response = await authService.RefreshAccessToken(refreshToken, new CancellationToken());
                    if (response != null)
                    {
                        context.Request.Headers["Authorization"] = "Bearer " + response.AccessToken;
                        context.Response.Cookies.Append("RefreshToken", response.RefreshToken);
                    }
                }
            }

            await _next(context);
        }
    }

}
