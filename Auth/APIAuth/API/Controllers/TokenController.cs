using Application.Interfaces;
using Application.User;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace APIAuth.API.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly TokenGenerator? _tokenGenerator;

        public TokenController() 
        {
            _tokenGenerator = new TokenGenerator();
        }
        [HttpPost("refreshtoken")]
        public IActionResult RefreshToken([FromBody]UserData reqUserDataSet)
        {
            var appUser = new AppUser 
            { 
                DisplayName = reqUserDataSet.DisplayName, 
            };
            var accessToken = _tokenGenerator?.RefreshToken(appUser, reqUserDataSet.RefreshToken);

            return Ok( new { AccessToken = accessToken.AccessToken });
        }
    }
}
