using AutoMapper;
using Common.Models;
using Library.Controllers;
using Library.Models;
using Library.Models.HttpResponses;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Player;
using Service.Services.Interfaces.PlayerInterfaces;
using WebAPI.Models.Player;

namespace WebAPI.Controllers.Player
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PlayerBoardController(IMapper mapper, IPlayerBoardService boardService) : WebApiController
    {
        [HttpGet("{playerId}/{displayed}")]
        [ProducesResponseType<HttpResponseBody<GetPlayer.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetPlayer.Response>>,
            NotFound<HttpResponseBody<GetPlayer.Response?>>,
            UnauthorizedHttpResult>>
            SetDisplayed(Guid playerId, bool displayed, CancellationToken cancellationToken)
        {
            var result = await boardService.SetDisplayed(playerId, displayed, cancellationToken);

            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<PlayerDto?, GetPlayer.Response?>(res => null));
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetPlayer.Response>));
        }

        [HttpGet("{playerId}/{teamId}/{position}")]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody>,
            BadRequest<HttpResponseBody>,
            NotFound<HttpResponseBody>,
            UnauthorizedHttpResult,
            InternalServerError<HttpResponseBody>>>
            InvitePlayerToTeam(Guid playerId, Guid teamId, int position, CancellationToken cancellationToken)
        {
            var result = await boardService.InvitePlayerToTeam(playerId, teamId, position, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody());
                }
                else
                {
                    return TypedResults.BadRequest(result.MapToHttpResponseBody());
                }
            }
            return TypedResults.Ok(result.MapToHttpResponseBody());
        }

        [HttpPost]
        [ProducesResponseType<HttpResponseBody<IEnumerable<GetPlayer.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<IEnumerable<GetPlayer.Response>>>,
            NotFound<HttpResponseBody<IEnumerable<GetPlayer.Response>>>>>
            GetFiltered(GetConditionalPlayer.Request request, CancellationToken cancellationToken)
        {
            var result = await boardService.GetFiltered(mapper.Map<ConditionalPlayerQuery>(request), cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<PlayerDto>, IEnumerable<GetPlayer.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetPlayer.Response>>));
        }

        [HttpPost("{pageSize}/{page}")]
        [ProducesResponseType<HttpResponseBody<PaginatedResult<GetPlayer.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<PaginatedResult<GetPlayer.Response>>>,
            NotFound<HttpResponseBody<PaginatedResult<GetPlayer.Response>>>>>
            GetPaginated(uint page, uint pageSize, GetConditionalPlayer.Request request, CancellationToken cancellationToken)
        {
            var result = await boardService.GetPaginated(mapper.Map<ConditionalPlayerQuery>(request), page, pageSize, cancellationToken);
            var response = result.MapToHttpResponseBody(mapper.Map<PaginatedResult<GetPlayer.Response>>);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(response);
            }

            return TypedResults.Ok(response);
        }
    }
}
