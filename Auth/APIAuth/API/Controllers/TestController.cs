using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Test()
        {
            return "succeed get";
        }
    }
}