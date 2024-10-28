﻿using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Team;
using Service.Services.Interfaces.TeamInterfaces;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamService(IMapper mapper, ITeamRepository teamRepo) : ITeamService
    {
        public async Task<TeamDto> Create(CreateTeamDto dto, CancellationToken cancellationToken = default)
        {
            var team = mapper.Map<Team>(dto);
            var createdTeam = await teamRepo.Add(team, cancellationToken);
            return mapper.Map<TeamDto>(createdTeam);
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return await teamRepo.Delete(id, cancellationToken);
        }

        public async Task<TeamDto?> Get(Guid id, CancellationToken cancellationToken)
        {
            var team = await teamRepo.Get(id, cancellationToken);
            return team == null ? null : mapper.Map<TeamDto>(team);
        }

        public async Task<ICollection<TeamDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<TeamDto>>(teams);
        }

        public async Task<TeamDto?> Update(UpdateTeamDto dto, CancellationToken cancellationToken = default)
        {
            var team = mapper.Map<Team>(dto);
            var updatedTeam = await teamRepo.Update(team, cancellationToken);
            return updatedTeam == null ? null : mapper.Map<TeamDto>(updatedTeam);
        }

        public async Task<TeamDto?> UpdateTeamPlayers(Guid id, ISet<TeamPlayerDto.Write> players, CancellationToken cancellationToken = default)
        {
            var updatedTeam = await teamRepo.UpdateTeamPlayers(id, mapper.Map<ISet<TeamPlayer>>(players), cancellationToken);
            return updatedTeam == null ? null : mapper.Map<TeamDto>(updatedTeam);
        }
    }
}
