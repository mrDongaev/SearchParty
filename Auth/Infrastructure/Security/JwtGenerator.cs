using Application.Interfaces;
using Application.User.Settings;
using Domain;
using Library.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly RsaSecurityKey _rsaPrivateKey;

        public JwtGenerator()
        {
            var privateKeyPem = EnvironmentUtils.GetEnvVariable("PRIVATE_KEY");

            _rsaPrivateKey = new RsaSecurityKey(AuthenticationUtils.LoadPrivateKeyFromPem(privateKeyPem));
        }

        public UserToken CreateJwtToken(AppUser user)
        {
            var refreshGenerator = new RefreshGenerator();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id)
            };

            var credentials = new SigningCredentials(_rsaPrivateKey, SecurityAlgorithms.RsaSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.Now.AddDays(7),

                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var accessToken = tokenHandler.WriteToken(token);

            var refreshToken = refreshGenerator.GenerateRefreshToken(user);

            return new Application.User.Settings.UserToken
            {
                AccessToken = accessToken,

                RefreshToken = refreshToken
            };
        }
    }
}