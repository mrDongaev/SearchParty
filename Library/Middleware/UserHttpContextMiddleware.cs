using Library.Services.Interfaces.UserContextInterfaces;
using System.Net;
using System.Security.Claims;

namespace Library.Middleware
{
    public class UserHttpContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserHttpContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserHttpContext userContext)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var nameId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (nameId != null)
                {
                    userContext.UserId = Guid.Parse(nameId);
                    userContext.AccessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    userContext.RefreshToken = context.Request.Cookies["RefreshToken"] ?? string.Empty;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }

            await _next(context);
        }
    }
}
