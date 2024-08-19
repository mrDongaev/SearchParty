using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;
using WebAPI.Contracts.Board;
using WebAPI.Contracts.Team;
using static Common.Models.ConditionalQuery;

namespace WebAPI.Controllers.Team
{
    [Route("api/[controller]/[action]")]
    public class TeamBoardController(IMapper mapper, IBoardService<TeamDto, TeamConditions> teamService) : WebApiController
    {
        [HttpPost("{id}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound>> SetDisplayed(Guid id, [FromBody] SetDisplay.Request request, CancellationToken cancellationToken)
        {
            var updatedPlayer = await teamService.SetDisplayed(id, (bool)request.Displayed, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(updatedPlayer));
        }
    }
}
