using AutoMapper;
using Library.Controllers;
using Library.Models.HttpResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Position;
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
        public async Task<Results<
            Ok<GetPosition.Response>,
            NotFound<HttpResponseBody<GetPosition.Response?>>>> Get(int id, CancellationToken cancellationToken)
        {
            var result = await positionService.Get(id, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<PositionDto?, GetPosition.Response?>(res => null));
            }

            return TypedResults.Ok(mapper.Map<GetPosition.Response>(result.Value));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetPosition.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetPosition.Response>>,
            NotFound<HttpResponseBody<IEnumerable<GetPosition.Response>>>>>
            GetAll(CancellationToken cancellationToken)
        {
            var result = await positionService.GetAll(cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<PositionDto>, IEnumerable<GetPosition.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetPosition.Response>>(result.Value));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetPosition.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetPosition.Response>>,
            NotFound<HttpResponseBody<IEnumerable<GetPosition.Response>>>>>
            GetRange(ICollection<int> ids, CancellationToken cancellationToken)
        {
            var result = await positionService.GetRange(ids, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<PositionDto>, IEnumerable<GetPosition.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetPosition.Response>>(result.Value));
        }
    }
}
