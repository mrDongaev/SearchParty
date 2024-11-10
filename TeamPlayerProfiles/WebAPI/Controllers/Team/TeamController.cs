using AutoMapper;
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
