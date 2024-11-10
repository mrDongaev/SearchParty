using AutoMapper;
using Common.Models;
using Library.Models;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces.PlayerInterfaces;
using WebAPI.Models.Player;
using WebAPI.Utils;

namespace WebAPI.Controllers.Player
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PlayerBoardController(IMapper mapper, IPlayerBoardService boardService, IServiceProvider serviceProvider, IUserHttpContext userContext) : WebApiController
    {
        [HttpPost("{playerId}/{displayed}")]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<GetPlayer.Response>, NotFound, UnauthorizedHttpResult>> SetDisplayed(Guid playerId, bool displayed, CancellationToken cancellationToken)
        {
            if (await OwnershipValidation.OwnsPlayer(serviceProvider, userContext.UserId, playerId, cancellationToken) == false)
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
            if (await OwnershipValidation.OwnsTeam(serviceProvider, userContext.UserId, teamId, cancellationToken) == false)
            {
                return TypedResults.Unauthorized();
            }
            if (await OwnershipValidation.OwnsPlayer(serviceProvider, userContext.UserId, playerId, cancellationToken) == true)
            {
                return TypedResults.BadRequest();
            }
            await boardService.InvitePlayerToTeam(playerId, teamId, cancellationToken);
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
