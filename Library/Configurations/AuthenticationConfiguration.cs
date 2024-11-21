using Library.Services.Implementations.AuthenticationServices;
using Library.Services.Implementations.UserContextServices;
using Library.Services.Interfaces.AuthenticationInterfaces;
using Library.Services.Interfaces.UserContextInterfaces;
using Library.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Library.Configurations
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
        {
            var publicKeyPem = EnvironmentUtils.GetEnvVariable("PUBLIC_KEY");
            var publicKey = new RsaSecurityKey(AuthenticationUtils.LoadPublicKeyFromPem(publicKeyPem));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = publicKey,
                };
            });

            services.AddScoped<IUserHttpContext, UserHttpContext>()
                .AddHttpClient<IAuthenticationService, AuthenticationService>(cfg =>
                {
                    cfg.BaseAddress = new Uri(EnvironmentUtils.GetEnvVariable("AUTHENTICATION_SERVICE_URL"));
                    cfg.DefaultRequestHeaders.Add("Accept", "text/plain");
                });
            return services;
        }
    }
}
