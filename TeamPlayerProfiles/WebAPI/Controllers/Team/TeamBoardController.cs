using AutoMapper;
using Common.Models;
using Library.Models;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Models.HttpResponses;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Service.Contracts.Player;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;
using Service.Services.Interfaces.PlayerInterfaces;
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
        [ProducesResponseType<HttpResponseBody<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetTeam.Response>>, 
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

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetTeam.Response>));
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
            UnauthorizedHttpResult>> 
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
        [ProducesResponseType<HttpResponseBody<IEnumerable<GetPlayer.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<IEnumerable<GetTeam.Response>>>, 
            NotFound<HttpResponseBody<IEnumerable<GetTeam.Response>>>>> 
            GetFiltered(GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var result = await boardService.GetFiltered(mapper.Map<ConditionalTeamQuery>(request), cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<TeamDto>, IEnumerable<GetTeam.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetTeam.Response>>));
        }

        [HttpPost("{pageSize}/{page}")]
        [ProducesResponseType<HttpResponseBody<PaginatedResult<GetTeam.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<PaginatedResult<GetTeam.Response>>>,
            NotFound<HttpResponseBody<PaginatedResult<GetTeam.Response>>>>>
            GetPaginated(uint page, uint pageSize, GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var result = await boardService.GetPaginated(mapper.Map<ConditionalTeamQuery>(request), page, pageSize, cancellationToken);
            var response = result.MapToHttpResponseBody(mapper.Map<PaginatedResult<GetTeam.Response>>);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(response);
            }

            return TypedResults.Ok(response);
        }
    }
}
