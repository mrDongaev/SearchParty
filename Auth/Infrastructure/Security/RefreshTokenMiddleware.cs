using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly RefreshGenerator _refreshGenerator;

        public RefreshTokenMiddleware(RequestDelegate next)
        {
            _refreshGenerator = new RefreshGenerator();

            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!ControllerAuthorize(context)) 
            {
                // Получить refresh token из заголовка Authorization
                string refreshToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Проверяем действительность refresh token
                if (!_refreshGenerator.ValidateRefreshToken(refreshToken))
                {
                    context.Response.StatusCode = 401;

                    await context.Response.WriteAsync("Invalid refresh token");

                    return;
                }

                // Получаем идентификатор пользователя из refresh token
                string userId = GetUserIdFromRefreshToken(refreshToken).ToLower();

                // Сравниваем идентификатор пользователя с идентификатором пользователя, который выполняет запрос
                if (userId != context?.User?.FindFirstValue(ClaimTypes.NameIdentifier).ToLower())
                {
                    context.Response.StatusCode = 401;

                    await context.Response.WriteAsync("Invalid user");

                    return;
                }
            }
            await _next(context);
        }

        public bool ControllerAuthorize(HttpContext context)
        {
            //Проверка, какой контроллер обрабатывает запрос
            var controllerActionDescriptor = context?.GetEndpoint().Metadata
                .GetMetadata<ControllerActionDescriptor>();

            var controllerType = controllerActionDescriptor?.ControllerTypeInfo;

            // Проверяю, есть ли атрибут [Authorize] на контроллере
            var authorizeAttribute = controllerType?.GetCustomAttributes(typeof(AuthorizeAttribute), true).FirstOrDefault();

            if (authorizeAttribute == null)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        private string GetUserIdFromRefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Читаем токен с помощью метода ReadToken и преобразуем его в объект JwtSecurityToken.
            // Это позволяет нам получить доступ к содержимому токена.
            var token = tokenHandler.ReadToken(refreshToken) as JwtSecurityToken;
            // Получаем список утверждений (claims) из токена.
            // Утверждения представляют собой пары ключ-значение, которые содержат информацию о пользователе.
            var claims = token.Claims.ToList();

            // Находим утверждение с типом NameId, которое содержит идентификатор пользователя.
            // Используем метод FirstOrDefault, чтобы получить первое утверждение, которое соответствует условию.
            var userIdClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId);

            return userIdClaim?.Value;
        }
    }
}
