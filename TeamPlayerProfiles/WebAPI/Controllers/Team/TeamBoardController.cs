using AutoMapper;
using Common.Models;
using Library.Models;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.TeamInterfaces;
using WebAPI.Models.Player;
using WebAPI.Models.Team;

namespace WebAPI.Controllers.Team
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TeamBoardController(IMapper mapper, ITeamBoardService teamBoardService, IServiceProvider serviceProvider, IUserHttpContext userContext) : WebApiController
    {
        [HttpPost("{teamId}/{displayed}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound, UnauthorizedHttpResult>> SetDisplayed(Guid teamId, bool displayed, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
            var userId = await teamService.GetProfileUserId(teamId, cancellationToken);
            if (!userId.HasValue) return TypedResults.NotFound();
            if (userId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var updatedPlayer = await teamBoardService.SetDisplayed(teamId, displayed, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(updatedPlayer));
        }

        [HttpPost("{teamId}/{playerId}/{position}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok, BadRequest, NotFound, UnauthorizedHttpResult>> SendTeamApplicationRequest(Guid teamId, Guid playerId, int position, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var playerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
            var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
            var playerUserId = await playerService.GetProfileUserId(playerId, cancellationToken);
            var teamUserId = await teamService.GetProfileUserId(teamId, cancellationToken);
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
            await teamBoardService.SendTeamApplicationRequest(message, cancellationToken);
            return TypedResults.Ok();
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetFiltered(GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var teams = await teamBoardService.GetFiltered(mapper.Map<ConditionalTeamQuery>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(teams));
        }

        [HttpPost("{pageSize}/{page}")]
        [ProducesResponseType<PaginatedResult<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetPaginated(uint page, uint pageSize, GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var teams = await teamBoardService.GetPaginated(mapper.Map<ConditionalTeamQuery>(request), page, pageSize, cancellationToken);
            return TypedResults.Ok(mapper.Map<PaginatedResult<GetTeam.Response>>(teams));
        }
    }
}
