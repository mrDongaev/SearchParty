using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Player;
using Service.Contracts.Team;
using Service.Services.Interfaces.TeamInterfaces;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamService(IMapper mapper, ITeamRepository teamRepo, IPlayerRepository playerRepo) : ITeamService
    {
        private static int maxCount = 5;

        public async Task<TeamDto> Create(CreateTeamDto dto, CancellationToken cancellationToken = default)
        {
            var team = mapper.Map<Team>(dto);
            var players = await playerRepo.GetRange(dto.PlayersInTeam.Select(pt => pt.PlayerId).ToList(), cancellationToken);
            TeamServiceUtils.CheckTeamValidity(players, dto.PlayersInTeam, team.UserId, 5);
            var createdTeam = await teamRepo.Add(team, cancellationToken);
            return mapper.Map<TeamDto>(createdTeam);
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return await teamRepo.Delete(id, cancellationToken);
        }

        public async Task<TeamDto?> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var team = await teamRepo.Get(id, cancellationToken);
            return team == null ? null : mapper.Map<TeamDto>(team);
        }

        public async Task<ICollection<TeamDto>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetRange(ids, cancellationToken);
            return mapper.Map<ICollection<TeamDto>>(teams);
        }

        public async Task<ICollection<TeamDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<TeamDto>>(teams);
        }

        public async Task<ICollection<TeamDto>> GetProfilesByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var teams = await teamRepo.GetProfilesByUserId(userId, cancellationToken);
            return mapper.Map<ICollection<TeamDto>>(teams);
        }

        public async Task<TeamDto?> Update(UpdateTeamDto dto, CancellationToken cancellationToken = default)
        {
            var team = mapper.Map<Team>(dto);
            ISet<TeamPlayer>? teamPlayers = null;
            if (dto.PlayersInTeam != null)
            {
                var players = await playerRepo.GetRange(dto.PlayersInTeam.Select(pt => pt.PlayerId).ToList(), cancellationToken);
                var existingTeam = await teamRepo.Get(dto.Id, cancellationToken);
                if (existingTeam == null) return null;
                TeamServiceUtils.CheckTeamValidity(players, dto.PlayersInTeam, existingTeam.UserId, 5);
                teamPlayers = mapper.Map<ISet<TeamPlayer>>(dto.PlayersInTeam);
            }
            var updatedTeam = await teamRepo.Update(team, teamPlayers, cancellationToken);
            return updatedTeam == null ? null : mapper.Map<TeamDto>(updatedTeam);
        }

        public async Task<Guid?> GetProfileUserId(Guid profileId, CancellationToken cancellationToken = default)
        {
            return await teamRepo.GetProfileUserId(profileId, cancellationToken);
        }
    }
}
