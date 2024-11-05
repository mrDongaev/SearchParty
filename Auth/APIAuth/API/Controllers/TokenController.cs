using Application.Exceptions;
using Application.Interfaces;
using Application.User;
using Application.User.Login;
using Application.User.Refresh;
using Azure.Core;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APIAuth.API.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : BaseController
    {
        private readonly JwtGenerator? _tokenGenerator;

        public TokenController() 
        {
            _tokenGenerator = new JwtGenerator();
        }

        [HttpPost("refreshtoken")]
        public async Task<ActionResult<UserData>> LoginAsync(RefreshQuery query)
        {
            try
            {
                var res = await Mediator.Send(query);

                return Ok(res);
            }
            catch (RestException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
