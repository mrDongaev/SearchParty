using AutoMapper;
using DataAccess.Repositories.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Player;
using Service.Services.Implementations.TeamServices;
using Service.Services.Interfaces.Common;
using Service.Services.Interfaces.PlayerInterfaces;
using WebAPI.Contracts.Board;
using WebAPI.Contracts.Player;
using static Common.Models.ConditionalQuery;
using static WebAPI.Contracts.Board.ConditionalProfile;

namespace WebAPI.Controllers.Player
{
    [Route("api/[controller]/[action]")]
    public class PlayerBoardController(IMapper mapper, IPlayerBoardService playerService) : WebApiController
    {
        [HttpPost("{playerId}/{displayed}")]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetPlayer.Response>, NotFound>> SetDisplayed(Guid playerId, bool displayed, CancellationToken cancellationToken)
        {
            var updatedPlayer = await playerService.SetDisplayed(playerId, displayed, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPlayer.Response>(updatedPlayer));
        }

        [HttpPost("{playerId}/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok, BadRequest>> InvitePlayerToTeam(Guid playerId, Guid teamId, CancellationToken cancellationToken)
        {
            await playerService.InvitePlayerToTeam(playerId, teamId, cancellationToken);
            return TypedResults.Ok();
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetFiltered(PlayerRequest request, CancellationToken cancellationToken)
        {
            var players = await playerService.GetFiltered(mapper.Map<PlayerConditions>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayer.Response>>(players));
        }

        [HttpPost("{pageSize}/{page}")]
        [ProducesResponseType<PaginatedResult<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetPaginated(uint page, uint pageSize, PlayerRequest request, CancellationToken cancellationToken)
        {
            var players = await playerService.GetPaginated(mapper.Map<PlayerConditions>(request), page, pageSize, cancellationToken);
            return TypedResults.Ok(mapper.Map<PaginatedResult<GetPlayer.Response>>(players));
        }
    }
}
