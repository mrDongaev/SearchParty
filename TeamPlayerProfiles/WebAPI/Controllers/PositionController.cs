using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using WebAPI.Contracts.Position;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PositionController(IPositionService positionService, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        public async Task<Results<Ok<GetPosition.Response>, NotFound>> Get(int id, CancellationToken cancellationToken)
        {
            var position = await positionService.Get(id, cancellationToken);
            return position == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPosition.Response>(position));
        }

        [HttpGet]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var positions = await positionService.GetAll(cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPosition.Response>>(positions));
        }
    }
}
