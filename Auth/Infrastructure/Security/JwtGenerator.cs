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

namespace Infrastructure.Security
{
    public class TokenGenerator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey _secretKey;

        private readonly SymmetricSecurityKey? _refreshKey;

        public TokenGenerator()
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN_KEY")
                ?? throw new ArgumentNullException("Token key not found in server environment variables"));

            _secretKey = new SymmetricSecurityKey(keyByte);

            byte[] refreshKeyByte = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("REFRESH_KEY")
                ?? throw new ArgumentNullException("Refresh token key not found in server environment variables"));

            _refreshKey = new SymmetricSecurityKey(refreshKeyByte);
        }

        public UserToken CreateJwtToken(AppUser user)
        {
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, user.UserName) };

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

            var refreshToken = GenerateRefreshToken(user);

            return new Application.User.Settings.UserToken
            {
                AccessToken = accessToken,

                RefreshToken = refreshToken
            };
        }
        public string GenerateRefreshToken(AppUser user)
        {
            // Создаем список утверждений (claims) для токена.
            // В данном случае мы добавляем только одно утверждение - имя пользователя.
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, user.UserName) };

            var credentials = new SigningCredentials(_refreshKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Устанавливаем субъект токена (то есть пользователя).
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.Now.AddDays(30),

                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public string RefreshToken(string refreshKey)
        {
            if (!ValidateRefreshToken(refreshKey)) 
            {
                throw new SecurityTokenException("Invalid refresh token");
            }
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(refreshKey, new TokenValidationParameters
                {
                    ValidateIssuer = false,

                    ValidateAudience = false,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = _refreshKey

                }, out var validatedToken);

                var newToken = CreateJwtToken(new AppUser { UserName = principal.FindFirstValue(JwtRegisteredClaimNames.NameId) });

                return newToken.RefreshToken;
            }
            catch (SecurityTokenException)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }
        }
        public bool ValidateRefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = false,

                    ValidateAudience = false,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = _refreshKey

                }, out var validatedToken);

                return true;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }
    }
}