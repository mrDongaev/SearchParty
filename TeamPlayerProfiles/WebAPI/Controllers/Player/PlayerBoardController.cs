using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Player;
using Service.Services.Interfaces.Common;
using WebAPI.Contracts.Board;
using WebAPI.Contracts.Player;
using WebAPI.Contracts.Team;

namespace WebAPI.Controllers.Player
{
    [Route("api/[controller]/[action]")]
    public class PlayerBoardController(IMapper mapper, IBoardService<PlayerDto> playerService) : WebApiController
    {
        [HttpPost("{id}")]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetPlayer.Response>, NotFound>> SetDisplayed(Guid id, [FromBody] SetDisplay.Request request, CancellationToken cancellationToken)
        {
            var updatedPlayer = await playerService.SetDisplay(id, (bool)request.Displayed, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPlayer.Response>(updatedPlayer));
        }
    }
}
