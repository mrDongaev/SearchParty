using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Common;
using Service.Contracts.Player;
using Service.Services.Interfaces.Common;
using WebAPI.Contracts.Player;
using WebAPI.Contracts.Team;

namespace WebAPI.Controllers.Player
{
    [Route("api/[controller]/[action]")]
    public class PlayerBoardController(IMapper mapper, IBoardService<PlayerDto> playerService) : WebApiController
    {
        [HttpPost]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        public async Task<IResult> SetDisplayed(SetDisplay.Request request, CancellationToken cancellationToken)
        {
            var updatedPlayer = await playerService.SetDisplay(request.Id, request.Displayed, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetPlayer.Response>(updatedPlayer));
        }
    }
}
