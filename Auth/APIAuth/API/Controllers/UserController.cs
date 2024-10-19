using Application.User.Login;
using Application.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.User.Registration;
using APIAuth.API.Controllers;

namespace APIAuth.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginAsync(LoginQuery query)
        {
            
            try
            {
                var res = await Mediator.Send(query);
                //Вернуть IActionResult
                return Ok(res);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("registration")]
        public async Task<ActionResult<User>> RegistrationAsync(RegistrationCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
