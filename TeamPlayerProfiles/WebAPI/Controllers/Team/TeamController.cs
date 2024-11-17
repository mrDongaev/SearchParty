using AutoMapper;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Team;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Interfaces.UserInterfaces;
using WebAPI.Models.Team;

namespace WebAPI.Controllers.Team
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TeamController(ITeamService teamService, IUserHttpContext userContext, IUserIdentityService userIdentity, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound>> Get(Guid id, CancellationToken cancellationToken)
        {
            var team = await teamService.Get(id, cancellationToken);
            return team == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(team));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetTeam.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var teams = await teamService.GetRange(ids, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(teams));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetTeam.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var teams = await teamService.GetAll(cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(teams));
        }

        [HttpPost]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        public async Task<IResult> Create(CreateTeam.Request request, CancellationToken cancellationToken)
        {
            var team = mapper.Map<CreateTeamDto>(request);
            var createdTeam = await teamService.Create(team, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetTeam.Response>(createdTeam));
        }

        [HttpPost("{id}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound, UnauthorizedHttpResult>> Update(Guid id, [FromBody] UpdateTeam.Request request, CancellationToken cancellationToken)
        {
            var userId = await userIdentity.GetTeamUserId(id, cancellationToken);
            if (userId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var tempTeam = mapper.Map<UpdateTeamDto>(request);
            tempTeam.Id = id;
            var updatedTeam = await teamService.Update(tempTeam, cancellationToken);
            return updatedTeam == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(updatedTeam));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok, NotFound, BadRequest, UnauthorizedHttpResult>> PushPlayerToTeam(PushPlayer.Request request, CancellationToken cancellationToken)
        {
            var playerUserId = await userIdentity.GetPlayerUserId(request.PlayerId, cancellationToken);
            var teamUserId = await userIdentity.GetTeamUserId(request.TeamId, cancellationToken);
            if (playerUserId == null || teamUserId == null)
            {
                return TypedResults.NotFound();
            }
            if (teamUserId != userContext.UserId || playerUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            if (request.MessageType != null && !Enum.IsDefined(request.MessageType.Value))
            {
                return TypedResults.BadRequest();
            }
            await teamService.PushPlayerToTeam(request.TeamId, request.PlayerId, request.Position, request.MessageId, request.MessageType, cancellationToken);
            return TypedResults.Ok();
        }

        [HttpPost("{teamId}/{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> PullPlayerFromTeam(Guid teamId, Guid playerId, CancellationToken cancellationToken)
        {
            var playerUserId = await userIdentity.GetPlayerUserId(playerId, cancellationToken);
            var teamUserId = await userIdentity.GetTeamUserId(teamId, cancellationToken);
            if (playerUserId == null || teamUserId == null)
            {
                return TypedResults.NotFound();
            }
            if (teamUserId != userContext.UserId || playerUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            await teamService.PullPlayerFromTeam(teamId, playerId, cancellationToken);
            return TypedResults.Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<bool>, UnauthorizedHttpResult>> Delete(Guid id, CancellationToken cancellationToken)
        {
            var userId = await userIdentity.GetTeamUserId(id, cancellationToken);
            if (userId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            return TypedResults.Ok(await teamService.Delete(id, cancellationToken));
        }
    }
}
