﻿using Service.Contracts.Team;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.TeamInterfaces
{
    public interface ITeamService : IProfileService<TeamDto, UpdateTeamDto, CreateTeamDto>
    {
        Task<TeamDto?> UpdateTeamPlayers(Guid id, ISet<TeamPlayerDto.Write> players, CancellationToken cancellationToken);
    }
}
