﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Team;
using Service.Services.Interfaces.TeamInterfaces;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Team;
using WebAPI.Validation;

namespace WebAPI.Controllers.Team
{
    [Route("api/[controller]/[action]")]
    public class TeamController(ITeamService teamService, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound>> Get(Guid id, CancellationToken cancellationToken)
        {
            var team = await teamService.Get(id, cancellationToken);
            return team == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(team));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetTeam.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var teams = await teamService.GetAll(cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(teams));
        }

        [HttpPost]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        public async Task<IResult> Create(CreateTeam.Request request, CancellationToken cancellationToken)
        {
            var team = mapper.Map<CreateTeamDto>(request);
            var createdTeam = await teamService.Create(team, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetTeam.Response>(createdTeam));
        }

        [HttpPost("{id}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound>> Update(Guid id, [FromBody] UpdateTeam.Request request, CancellationToken cancellationToken)
        {
            var tempTeam = mapper.Map<UpdateTeamDto>(request);
            tempTeam.Id = id;
            var updatedTeam = await teamService.Update(tempTeam, cancellationToken);
            return updatedTeam == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(updatedTeam));
        }

        [HttpPost("{id}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound>> UpdateTeamPlayer(Guid id, [FromBody, MaxLength(5), UniqueTeamPositions] ISet<UpdateTeamPlayers.Request> request, CancellationToken cancellationToken)
        {
            var updatedTeam = await teamService.UpdateTeamPlayers(id, mapper.Map<ISet<TeamPlayerDto.Write>>(request), cancellationToken);
            return updatedTeam == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(updatedTeam));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            return TypedResults.Ok(await teamService.Delete(id, cancellationToken));
        }
    }
}
