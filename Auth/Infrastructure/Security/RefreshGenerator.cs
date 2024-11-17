using Application.Interfaces;
using Application.User;
using Domain;
using Library.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class RefreshGenerator : IRefreshGenerator
    {
        private readonly SymmetricSecurityKey? _refreshKey;

        public RefreshGenerator()
        {
            byte[] refreshKeyByte = Encoding.UTF8.GetBytes(EnvironmentUtils.GetEnvVariable("REFRESH_KEY"));

            _refreshKey = new SymmetricSecurityKey(refreshKeyByte);
        }
        public string GenerateRefreshToken(AppUser user)
        {
            // Создаем список утверждений (claims) для токена.
            // В данном случае мы добавляем только одно утверждение - имя пользователя.
            var claims = new List<Claim> 
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

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
        public UserData RefreshToken(AppUser appUser, UserData userData)
        {
            if (userData.RefreshToken == string.Empty || userData.RefreshToken == null)
            {
                throw new SecurityTokenException("You did not transfer the refresh token for the update");
            }
            if (!ValidateRefreshToken(userData.RefreshToken))
            {
                throw new SecurityTokenException("Invalid refresh token");
            }
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(userData.RefreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = false,

                    ValidateAudience = false,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = _refreshKey

                }, out var validatedToken);

                var jwtGenerator = new JwtGenerator();

                var newToken = jwtGenerator.CreateJwtToken(appUser);

                userData.RefreshToken = newToken.RefreshToken;

                userData.AccessToken = newToken.AccessToken;

                return userData;
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
        public string DecodeRefreshToken(string refreshToken, string parameter)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.ReadJwtToken(refreshToken);
            
            var result = token.Claims.FirstOrDefault(c => c.Type == parameter)?.Value;

            if (result == null)
            {
                return string.Empty;
            }
            else 
            {
                return result;
            }
        }
    }
}
