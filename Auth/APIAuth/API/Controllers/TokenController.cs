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
        public async Task<ActionResult<AuthenticatedUser>> LoginAsync(RefreshQuery query)
        {
            try
            {
                var userData = await Mediator.Send(query);

                var res = new AuthenticatedUser
                {
                    Id = userData.Id,
                    AccessToken = userData.AccessToken,
                    DisplayName = userData.DisplayName,
                    Email = userData.Email,
                    RefreshToken = userData.RefreshToken,
                };

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
