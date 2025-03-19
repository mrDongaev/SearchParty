using APIAuth.API.Controllers;
using Application.Exceptions;
using Application.User;
using Application.User.Login;
using Application.User.Registration;
using Microsoft.AspNetCore.Mvc;

namespace APIAuth.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticatedUser>> LoginAsync(LoginQuery query)
        {
            //try catch
            try
            {
                var userData = await Mediator.Send(query);

                var res = new AuthenticatedUser
                {
                    Id = userData.Id,
                    AccessToken = userData.AccessToken,
                    RefreshToken = userData.RefreshToken,
                    DisplayName = userData.DisplayName,
                    Email = userData.Email,
                };
                //Вернуть IActionResult
                return Ok(res);
            }
            catch (RestException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }

        }

        [HttpPost("registration")]
        public async Task<ActionResult<AuthenticatedUser>> RegistrationAsync(RegistrationQuery command)
        {
            var userData = await Mediator.Send(command);
            var res = new AuthenticatedUser
            {
                Id = userData.Id,
                AccessToken = userData.AccessToken,
                RefreshToken = userData.RefreshToken,
                DisplayName = userData.DisplayName,
                Email = userData.Email,
            };
            //Вернуть IActionResult
            return Ok(res);
        }
    }
}
