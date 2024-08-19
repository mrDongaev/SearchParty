using AutoMapper;
using Common.Models;
using DataAccess.Repositories.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Player;
using Service.Services.Interfaces.Common;
using WebAPI.Contracts.Board;
using WebAPI.Contracts.Player;
using static Common.Models.ConditionalQuery;
using static WebAPI.Contracts.Board.ConditionalProfile;

namespace WebAPI.Controllers.Player
{
    [Route("api/[controller]/[action]")]
    public class PlayerBoardController(IMapper mapper, IBoardService<PlayerDto, PlayerConditions> playerService) : WebApiController
    {
        [HttpPost("{id}")]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetPlayer.Response>, NotFound>> SetDisplayed(Guid id, [FromBody] SetDisplay.Request request, CancellationToken cancellationToken)
        {
            var updatedPlayer = await playerService.SetDisplayed(id, (bool)request.Displayed, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPlayer.Response>(updatedPlayer));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetFiltered(PlayerRequest request, CancellationToken cancellationToken)
        {
            var updatedPlayer = await playerService.GetFiltered(mapper.Map<PlayerConditions>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayer.Response>>(updatedPlayer));
        }

        [HttpPost]
        [ProducesResponseType<PaginatedResult<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetPaginated(PaginatedPlayerRequest request, CancellationToken cancellationToken)
        {
            var updatedPlayer = await playerService.GetPaginated(mapper.Map<PlayerConditions>(request), (int) request.Page, (int) request.PageSize, cancellationToken);
            return TypedResults.Ok(mapper.Map<PaginatedResult<GetPlayer.Response>>(updatedPlayer));
        }
    }
}
