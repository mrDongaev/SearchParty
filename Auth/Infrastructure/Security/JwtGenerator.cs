using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Application.User.Settings;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Application.User;
using Library.Utils;

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