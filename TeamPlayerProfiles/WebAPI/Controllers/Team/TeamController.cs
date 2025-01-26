using AutoMapper;
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
        [ProducesResponseType<HttpResponseBody<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetTeam.Response>>,
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

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetTeam.Response>));
        }

        [HttpPost]
        [ProducesResponseType<HttpResponseBody<IEnumerable<GetTeam.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<IEnumerable<GetTeam.Response>>>,
            NotFound<HttpResponseBody<IEnumerable<GetTeam.Response>>>>>
            GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var result = await teamService.GetRange(ids, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<TeamDto>, IEnumerable<GetTeam.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetTeam.Response>>));
        }

        [HttpGet]
        [ProducesResponseType<HttpResponseBody<IEnumerable<GetTeam.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await teamService.GetAll(cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<TeamDto>, IEnumerable<GetTeam.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetTeam.Response>>));
        }

        [HttpGet]
        [ProducesResponseType<HttpResponseBody<IEnumerable<GetTeam.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<IEnumerable<GetTeam.Response>>>,
            NotFound<HttpResponseBody<IEnumerable<GetTeam.Response>>>>>
            GetTeamsOfUser(CancellationToken cancellationToken)
        {
            var result = await teamService.GetProfilesByUserId(userContext.UserId, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<TeamDto>, IEnumerable<GetTeam.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetTeam.Response>>));
        }

        [HttpPost]
        [ProducesResponseType<HttpResponseBody<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        public async Task<Results<
            Ok<HttpResponseBody<GetTeam.Response>>,
            BadRequest<HttpResponseBody<GetTeam.Response?>>>>
            Create(CreateTeam.Request request, CancellationToken cancellationToken)
        {
            var team = mapper.Map<CreateTeamDto>(request);
            var result = await teamService.Create(team, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.MapToHttpResponseBody<TeamDto, GetTeam.Response?>(res => null));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetTeam.Response>));
        }

        [HttpPost("{id}")]
        [ProducesResponseType<HttpResponseBody<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetTeam.Response>>,
            NotFound<HttpResponseBody<GetTeam.Response?>>,
            BadRequest<HttpResponseBody<GetTeam.Response?>>,
            UnauthorizedHttpResult>>
            Update(Guid id, [FromBody] UpdateTeam.Request request, CancellationToken cancellationToken)
        {
            var tempTeam = mapper.Map<UpdateTeamDto>(request);
            tempTeam.Id = id;
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

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetTeam.Response>));
        }

        [HttpPost]
        [ProducesResponseType<HttpResponseBody<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetTeam.Response>>,
            NotFound<HttpResponseBody<GetTeam.Response?>>,
            BadRequest<HttpResponseBody<GetTeam.Response?>>,
            UnauthorizedHttpResult>>
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

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetTeam.Response>));
        }

        [HttpGet("{teamId}/{playerId}")]
        [ProducesResponseType<HttpResponseBody<GetTeam.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetTeam.Response>>,
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

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetTeam.Response>));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<HttpResponseBody<bool>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody<bool>>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<bool>>,
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
                    return TypedResults.NotFound(result.MapToHttpResponseBody(res => res.Value));
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(res => res.Value));
        }
    }
}
