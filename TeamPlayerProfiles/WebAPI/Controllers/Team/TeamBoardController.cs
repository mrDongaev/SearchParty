using AutoMapper;
using DataAccess.Repositories.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;
using Service.Services.Interfaces.TeamInterfaces;
using System.ComponentModel.DataAnnotations;
using WebAPI.Contracts.Board;
using WebAPI.Contracts.Player;
using WebAPI.Contracts.Team;
using static Common.Models.ConditionalQuery;
using static WebAPI.Contracts.Board.ConditionalProfile;

namespace WebAPI.Controllers.Team
{
    [Route("api/[controller]/[action]")]
    public class TeamBoardController(IMapper mapper, ITeamBoardService teamService) : WebApiController
    {
        [HttpPost("{teamId}/{displayed}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound>> SetDisplayed(Guid teamId, bool displayed, CancellationToken cancellationToken)
        {
            var updatedPlayer = await teamService.SetDisplayed(teamId, displayed, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(updatedPlayer));
        }

        [HttpPost("{teamId}/{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok, BadRequest>> SendTeamAccessionRequest(Guid teamId, Guid playerId, CancellationToken cancellationToken)
        {
            await teamService.SendTeamAccessionRequest(teamId, playerId, cancellationToken);
            return TypedResults.Ok();
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetFiltered(TeamRequest request, CancellationToken cancellationToken)
        {
            var teams = await teamService.GetFiltered(mapper.Map<TeamConditions>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(teams));
        }

        [HttpPost("{pageSize}/{page}")]
        [ProducesResponseType<PaginatedResult<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetPaginated(uint page, uint pageSize, TeamRequest request, CancellationToken cancellationToken)
        {
            var teams = await teamService.GetPaginated(mapper.Map<TeamConditions>(request), page, pageSize, cancellationToken);
            return TypedResults.Ok(mapper.Map<PaginatedResult<GetTeam.Response>>(teams));
        }
    }
}
