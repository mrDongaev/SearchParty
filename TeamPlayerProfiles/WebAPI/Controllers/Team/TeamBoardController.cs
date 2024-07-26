using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Common;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;
using WebAPI.Contracts.Team;

namespace WebAPI.Controllers.Team
{
    [Route("api/[controller]/[action]")]
    public class TeamBoardController(IMapper mapper, IBoardService<TeamDto> teamService) : WebApiController
    {
        [HttpPost]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        public async Task<IResult> SetDisplayed(SetDisplay.Request request, CancellationToken cancellationToken)
        {
            var updatedPlayer = await teamService.SetDisplay(request.Id, request.Displayed, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetTeam.Response>(updatedPlayer));
        }
    }
}
