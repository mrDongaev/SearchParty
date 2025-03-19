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
using Service.Contracts.Team;
using Service.Services.Interfaces.TeamInterfaces;
using WebAPI.Models.Player;
using WebAPI.Models.Team;

namespace WebAPI.Controllers.Team
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TeamBoardController(IMapper mapper, ITeamBoardService boardService) : WebApiController
    {
        [HttpGet("{teamId}/{displayed}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetTeam.Response>,
            NotFound<HttpResponseBody<GetTeam.Response?>>,
            UnauthorizedHttpResult>>
            SetDisplayed(Guid teamId, bool displayed, CancellationToken cancellationToken)
        {
            var result = await boardService.SetDisplayed(teamId, displayed, cancellationToken);

            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<TeamDto?, GetTeam.Response?>(res => null));
                }
            }

            return TypedResults.Ok(mapper.Map<GetTeam.Response>(result.Value));
        }

        [HttpGet("{teamId}/{playerId}/{position}")]
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
            SendTeamApplicationRequest(Guid teamId, Guid playerId, int position, CancellationToken cancellationToken)
        {
            var result = await boardService.SendTeamApplicationRequest(playerId, teamId, position, cancellationToken);
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
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetTeam.Response>>,
            NotFound<HttpResponseBody<IEnumerable<GetTeam.Response>>>>>
            GetFiltered(GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var result = await boardService.GetFiltered(mapper.Map<ConditionalTeamQuery>(request), cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<TeamDto>, IEnumerable<GetTeam.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(result.Value));
        }

        [HttpPost("{pageSize}/{page}")]
        [ProducesResponseType<PaginatedResult<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<PaginatedResult<GetTeam.Response>>,
            NotFound<HttpResponseBody<PaginatedResult<GetTeam.Response>>>>>
            GetPaginated(uint page, uint pageSize, GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var result = await boardService.GetPaginated(mapper.Map<ConditionalTeamQuery>(request), page, pageSize, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody(res => new PaginatedResult<GetTeam.Response>() { PageSize = pageSize }));
            }

            return TypedResults.Ok(mapper.Map<PaginatedResult<GetTeam.Response>>(result.Value));
        }
    }
}
