using AutoMapper;
using Common.Models;
using Library.Models;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.UserInterfaces;
using WebAPI.Models.Player;

namespace WebAPI.Controllers.Player
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PlayerBoardController(IMapper mapper, IPlayerBoardService boardService, IUserIdentityService userIdentity, IUserHttpContext userContext) : WebApiController
    {
        [HttpPost("{playerId}/{displayed}")]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<GetPlayer.Response>, NotFound, UnauthorizedHttpResult>> SetDisplayed(Guid playerId, bool displayed, CancellationToken cancellationToken)
        {
            var userId = await userIdentity.GetPlayerUserId(playerId, cancellationToken);
            if (userId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var updatedPlayer = await boardService.SetDisplayed(playerId, displayed, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPlayer.Response>(updatedPlayer));
        }

        [HttpPost("{playerId}/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok, BadRequest, UnauthorizedHttpResult>> InvitePlayerToTeam(Guid playerId, Guid teamId, CancellationToken cancellationToken)
        {
            var teamUserId = await userIdentity.GetTeamUserId(teamId, cancellationToken);
            if (teamUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var playerUserId = await userIdentity.GetPlayerUserId(playerId, cancellationToken);
            if (playerUserId == userContext.UserId)
            {
                return TypedResults.BadRequest();
            }
            var message = new Message()
            {
                SenderId = teamId,
                SendingUserId = userContext.UserId,
                AcceptorId = playerId,
                AcceptingUserId = userContext.UserId,
                MessageType = MessageType.TeamInvitation,
            };
            await boardService.InvitePlayerToTeam(message, cancellationToken);
            return TypedResults.Ok();
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetFiltered(GetConditionalPlayer.Request request, CancellationToken cancellationToken)
        {
            var players = await boardService.GetFiltered(mapper.Map<ConditionalPlayerQuery>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayer.Response>>(players));
        }

        [HttpPost("{pageSize}/{page}")]
        [ProducesResponseType<PaginatedResult<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetPaginated(uint page, uint pageSize, GetConditionalPlayer.Request request, CancellationToken cancellationToken)
        {
            var players = await boardService.GetPaginated(mapper.Map<ConditionalPlayerQuery>(request), page, pageSize, cancellationToken);
            return TypedResults.Ok(mapper.Map<PaginatedResult<GetPlayer.Response>>(players));
        }
    }
}
