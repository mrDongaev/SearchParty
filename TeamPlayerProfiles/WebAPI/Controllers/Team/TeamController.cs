using AutoMapper;
using Library.Controllers;
using Library.Models.HttpResponses;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Team;
using Service.Services.Interfaces.TeamInterfaces;
using WebAPI.Models.Team;

namespace WebAPI.Controllers.Team
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TeamController(ITeamService teamService, IUserHttpContext userContext, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetTeam.Response>,
            NotFound<HttpResponseBody<GetTeam.Response?>>,
            UnauthorizedHttpResult>>
            Get(Guid id, CancellationToken cancellationToken)
        {
            var result = await teamService.Get(id, cancellationToken);

            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<TeamDto?, GetTeam.Response?>(res => null));
                }
            }

            return TypedResults.Ok(mapper.Map<GetTeam.Response>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetTeam.Response>>,
            NotFound<HttpResponseBody<IEnumerable<GetTeam.Response>>>>>
            GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var result = await teamService.GetRange(ids, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<TeamDto>, IEnumerable<GetTeam.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(result.Value));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await teamService.GetAll(cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<TeamDto>, IEnumerable<GetTeam.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(result.Value));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetTeam.Response>>,
            NotFound<HttpResponseBody<IEnumerable<GetTeam.Response>>>>>
            GetTeamsOfUser(CancellationToken cancellationToken)
        {
            var result = await teamService.GetProfilesByUserId(userContext.UserId, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<TeamDto>, IEnumerable<GetTeam.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        public async Task<Results<
            Ok<GetTeam.Response>,
            BadRequest<HttpResponseBody<GetTeam.Response?>>>>
            Create(CreateTeam.Request request, CancellationToken cancellationToken)
        {
            var team = mapper.Map<CreateTeamDto>(request);
            var result = await teamService.Create(team, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.MapToHttpResponseBody<TeamDto, GetTeam.Response?>(res => null));
            }

            return TypedResults.Ok(mapper.Map<GetTeam.Response>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetTeam.Response>,
            NotFound<HttpResponseBody<GetTeam.Response?>>,
            BadRequest<HttpResponseBody<GetTeam.Response?>>,
            UnauthorizedHttpResult>>
            Update([FromBody] UpdateTeam.Request request, CancellationToken cancellationToken)
        {
            var tempTeam = mapper.Map<UpdateTeamDto>(request);
            var result = await teamService.Update(tempTeam, cancellationToken);

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
                else
                {
                    return TypedResults.BadRequest(result.MapToHttpResponseBody<TeamDto?, GetTeam.Response?>(res => null));
                }
            }

            return TypedResults.Ok(mapper.Map<GetTeam.Response>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetTeam.Response>,
            NotFound<HttpResponseBody<GetTeam.Response?>>,
            BadRequest<HttpResponseBody<GetTeam.Response?>>,
            UnauthorizedHttpResult,
            InternalServerError<HttpResponseBody<GetTeam.Response?>>>>
            PushPlayerToTeam(PushPlayer.Request request, CancellationToken cancellationToken)
        {
            var result = await teamService.PushPlayerToTeam(request.TeamId, request.PlayerId, request.Position, request.MessageId, request.MessageType, cancellationToken);

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
                else
                {
                    return TypedResults.BadRequest(result.MapToHttpResponseBody<TeamDto?, GetTeam.Response?>(res => null));
                }
            }

            return TypedResults.Ok(mapper.Map<GetTeam.Response>(result.Value));
        }

        [HttpGet("{teamId}/{playerId}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetTeam.Response>,
            NotFound<HttpResponseBody<GetTeam.Response?>>,
            BadRequest<HttpResponseBody<GetTeam.Response?>>,
            UnauthorizedHttpResult>>
            PullPlayerFromTeam(Guid teamId, Guid playerId, CancellationToken cancellationToken)
        {
            var result = await teamService.PullPlayerFromTeam(teamId, playerId, cancellationToken);

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
                else
                {
                    return TypedResults.BadRequest(result.MapToHttpResponseBody<TeamDto?, GetTeam.Response?>(res => null));
                }
            }

            return TypedResults.Ok(mapper.Map<GetTeam.Response>(result.Value));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody<bool>>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<bool>,
            NotFound<HttpResponseBody<bool>>,
            UnauthorizedHttpResult>> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await teamService.Delete(id, cancellationToken);

            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody(res => false));
                }
            }

            return TypedResults.Ok(result.Value);
        }
    }
}
