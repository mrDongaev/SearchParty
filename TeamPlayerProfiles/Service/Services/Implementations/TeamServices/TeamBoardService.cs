﻿using AutoMapper;
using Common.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Models;
using Library.Models.API.UserProfiles.User;
using Library.Models.Enums;
using Library.Models.QueryConditions;
using Service.Contracts.Team;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Interfaces.UserProfilesInterfaces;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamBoardService(IMapper mapper, ITeamRepository teamRepo) : ITeamBoardService
    {
        public async Task<TeamDto?> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken)
        {
            var team = new Team() { Id = id, Displayed = displayed };
            var updatedTeam = await teamRepo.Update(team, cancellationToken);
            return updatedTeam == null ? null : mapper.Map<TeamDto>(updatedTeam);
        }

        public async Task SendTeamAccessionRequest(Guid teamId, Guid requestingPlayerId, CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            throw new NotImplementedException();
        }

        public async Task<ICollection<TeamDto>> GetFiltered(ConditionalTeamQuery query, CancellationToken cancellationToken)
        {
            var teams = await teamRepo.GetConditionalTeamRange(query, cancellationToken);
            return mapper.Map<ICollection<TeamDto>>(teams);
        }

        public async Task<PaginatedResult<TeamDto>> GetPaginated(ConditionalTeamQuery query, uint page, uint pageSize, CancellationToken cancellationToken)
        {
            var teams = await teamRepo.GetPaginatedTeamRange(query, page, pageSize, cancellationToken);
            return mapper.Map<PaginatedResult<TeamDto>>(teams);
        }
    }
}
