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

namespace Infrastructure.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey _secretKey;

        public JwtGenerator()
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN_KEY")
                ?? throw new ArgumentNullException("Token key not found in server environment variables"));

            _secretKey = new SymmetricSecurityKey(keyByte);
        }

        public UserToken CreateJwtToken(AppUser user)
        {
            var refreshGenerator = new RefreshGenerator();

            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, user.UserName, user.Email) };

            var credentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha512Signature);

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