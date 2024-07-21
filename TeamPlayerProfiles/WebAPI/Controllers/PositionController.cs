using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    public class PositionController(IPositionService positionService) : WebApiController
    {
        [HttpGet(Name)]
    }
}
