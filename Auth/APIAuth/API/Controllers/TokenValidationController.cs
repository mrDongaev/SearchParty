using Application.User.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenValidationController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public TokenValidationController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] PublicTokenRequest tokenRequest)
        {
            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.Token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                // Дополнительная проверка, если необходимо
                if (!IsTokenValid(validatedToken))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                return Ok(new { message = "Token is valid", claims = principal.Claims.Select(c => new { c.Type, c.Value }) });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Token validation failed", error = ex.Message });
            }
        }

        private bool IsTokenValid(SecurityToken validatedToken)
        {
            // Вы можете добавить дополнительные проверки токена здесь, если необходимо
            return true;
        }
    }
}