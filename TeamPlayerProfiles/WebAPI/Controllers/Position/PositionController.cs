using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces.PositionInterfaces;
using WebAPI.Models.Position;

namespace WebAPI.Controllers.Position
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PositionController(IPositionService positionService, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetPosition.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetPosition.Response>, NotFound>> Get(int id, CancellationToken cancellationToken)
        {
            var position = await positionService.Get(id, cancellationToken);
            return position == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPosition.Response>(position));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetPosition.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var positions = await positionService.GetAll(cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPosition.Response>>(positions));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPosition.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetRange(ICollection<int> ids, CancellationToken cancellationToken)
        {
            var positions = await positionService.GetRange(ids, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPosition.Response>>(positions));
        }
    }
}
