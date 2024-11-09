using Application.User.Login;
using Application.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.User.Registration;
using APIAuth.API.Controllers;
using Application.Exceptions;

namespace APIAuth.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserData>> LoginAsync(LoginQuery query)
        {
            //try catch
            try
            {
                var res = await Mediator.Send(query);
                //Вернуть IActionResult
                return Ok(res);
            }
            catch(RestException ex) 
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
        public async Task RegistrationAsync(RegistrationQuery command)
        {
            await Mediator.Send(command);
        }
    }
}
