using AutoMapper;
using Common.Models;
using Library.Models;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Interfaces.UserInterfaces;
using WebAPI.Models.Player;
using WebAPI.Models.Team;

namespace WebAPI.Controllers.Team
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TeamBoardController(IMapper mapper, ITeamBoardService teamService, IUserHttpContext userContext, IUserIdentityService userIdentity) : WebApiController
    {
        [HttpPost("{teamId}/{displayed}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound, UnauthorizedHttpResult>> SetDisplayed(Guid teamId, bool displayed, CancellationToken cancellationToken)
        {
            var userId = await userIdentity.GetTeamUserId(teamId, cancellationToken);
            if (!userId.HasValue) return TypedResults.NotFound();
            if (userId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var updatedPlayer = await teamService.SetDisplayed(teamId, displayed, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(updatedPlayer));
        }

        [HttpPost("{teamId}/{playerId}/{position}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok, BadRequest, NotFound, UnauthorizedHttpResult>> SendTeamApplicationRequest(Guid teamId, Guid playerId, int position, CancellationToken cancellationToken)
        {
            var playerUserId = await userIdentity.GetPlayerUserId(playerId, cancellationToken);
            var teamUserId = await userIdentity.GetTeamUserId(teamId, cancellationToken);
            if (playerUserId == null || teamUserId == null)
            {
                return TypedResults.NotFound();
            }
            if (playerUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            if (teamUserId == userContext.UserId || !Enum.IsDefined(typeof(PositionName), position))
            {
                return TypedResults.BadRequest();
            }
            var message = new ProfileMessageSubmitted()
            {
                SenderId = playerId,
                SendingUserId = userContext.UserId,
                AcceptorId = teamId,
                AcceptingUserId = (Guid)teamUserId,
                PositionName = (PositionName)position,
                MessageType = MessageType.TeamApplication,
            };
            await teamService.SendTeamApplicationRequest(message, cancellationToken);
            return TypedResults.Ok();
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetFiltered(GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var teams = await teamService.GetFiltered(mapper.Map<ConditionalTeamQuery>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(teams));
        }

        [HttpPost("{pageSize}/{page}")]
        [ProducesResponseType<PaginatedResult<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetPaginated(uint page, uint pageSize, GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var teams = await teamService.GetPaginated(mapper.Map<ConditionalTeamQuery>(request), page, pageSize, cancellationToken);
            return TypedResults.Ok(mapper.Map<PaginatedResult<GetTeam.Response>>(teams));
        }
    }
}
