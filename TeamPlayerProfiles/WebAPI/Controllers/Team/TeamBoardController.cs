using AutoMapper;
using DataAccess.Repositories.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;
using WebAPI.Contracts.Board;
using WebAPI.Contracts.Player;
using WebAPI.Contracts.Team;
using static Common.Models.ConditionalQuery;
using static WebAPI.Contracts.Board.ConditionalProfile;

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

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetFiltered(TeamRequest request, CancellationToken cancellationToken)
        {
            var teams = await teamService.GetFiltered(mapper.Map<TeamConditions>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(teams));
        }

        [HttpPost]
        [ProducesResponseType<PaginatedResult<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetPaginated([FromQuery(Name = "page")] uint page, [FromQuery(Name = "page-size")] uint pageSize, TeamRequest request, CancellationToken cancellationToken)
        {
            var teams = await teamService.GetPaginated(mapper.Map<TeamConditions>(request), page, pageSize, cancellationToken);
            return TypedResults.Ok(mapper.Map<PaginatedResult<GetTeam.Response>>(teams));
        }
    }
}
