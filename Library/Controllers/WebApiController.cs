﻿using Library.Models.HttpResponses;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [ApiController]
    [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<HttpResponseBody>(StatusCodes.Status500InternalServerError)]
    public abstract class WebApiController() : ControllerBase()
    {
    }
}
