using Common.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories.Models;
using DataAccess.Utils;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class TeamRepository : Repository<Team, Guid>, ITeamRepository
    {
        private readonly DbSet<Team> _teams;

        public TeamRepository(TeamPlayerProfilesContext context) : base(context)
        {
            _teams = _context.Teams;
        }

        public override async Task<ICollection<Team>> GetAll(CancellationToken cancellationToken)
        {
            return await _teams.GetEntities(true)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Team?> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _teams.GetEntities(true)
                .SingleAsync(t => t.Id == id, cancellationToken);
        }

        public override async Task<Team> Add(Team team, CancellationToken cancellationToken)
        {
            team.Id = Guid.NewGuid();
            foreach (var tp in team.TeamPlayers)
            {
                tp.TeamId = team.Id;
            }
            team.PlayerCount = team.TeamPlayers.Count;
            await base.Add(team, cancellationToken);
            return await Get(team.Id, cancellationToken);
        }

        public override async Task<Team?> Update(Team team, CancellationToken cancellationToken)
        {
            var existingTeam = await _teams
                .Include(t => t.TeamPlayers)
                .SingleOrDefaultAsync(t => t.Id == team.Id, cancellationToken);
            if (existingTeam == null)
            {
                return null;
            }
            if (team.Name != null) existingTeam.Name = team.Name;
            if (team.Description != null) existingTeam.Description = team.Description;
            if (team.Displayed != null) existingTeam.Displayed = team.Displayed;
            if (_context.Entry(existingTeam).State == EntityState.Modified)
            {
                existingTeam.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync(cancellationToken);
            return await Get(team.Id, cancellationToken);
        }

        public async Task<Team?> UpdateTeamPlayers(Guid id, ISet<TeamPlayer> players, CancellationToken cancellationToken)
        {
            var existingTeam = await _teams
                .Include(t => t.TeamPlayers)
                .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (existingTeam == null)
            {
                return null;
            }
            var existingPlayerIds = existingTeam.TeamPlayers.Select(tp => tp.PlayerId);
            var updatedPlayerIds = players.Select(tp => tp.PlayerId);
            var playerIdsToAdd = updatedPlayerIds.Except(existingPlayerIds);
            var playerIdsToRemove = existingPlayerIds.Except(updatedPlayerIds);
            var playerIdsToUpdate = existingPlayerIds.Except(playerIdsToRemove);
            if (playerIdsToRemove.Any())
            {
                var teamPlayersToRemove = existingTeam.TeamPlayers
                    .Where(tp => playerIdsToRemove.Contains(tp.PlayerId))
                    .ToList();
                foreach (var teamPlayer in teamPlayersToRemove)
                {
                    existingTeam.TeamPlayers.Remove(teamPlayer);
                }
                _context.Entry(existingTeam).State = EntityState.Modified;
            }
            if (playerIdsToAdd.Any())
            {
                var teamPlayersToAdd = players
                    .Where(tp => playerIdsToAdd.Contains(tp.PlayerId))
                    .ToList();
                foreach (var teamPlayer in teamPlayersToAdd)
                {
                    teamPlayer.TeamId = existingTeam.Id;
                    existingTeam.TeamPlayers.Add(teamPlayer);
                }
                _context.Entry(existingTeam).State = EntityState.Modified;
            }
            if (playerIdsToUpdate.Any())
            {
                var updatedPlayers = players
                    .Where(tp => playerIdsToUpdate.Contains(tp.PlayerId))
                    .ToList();
                foreach (var updatedPlayer in updatedPlayers)
                {
                    var player = existingTeam.TeamPlayers.SingleOrDefault(tp => tp.PlayerId.Equals(updatedPlayer.PlayerId));
                    if (player != null) player.PositionId = updatedPlayer.PositionId;
                }
                _context.Entry(existingTeam).State = EntityState.Modified;
            }
            if (_context.Entry(existingTeam).State == EntityState.Modified)
            {
                existingTeam.UpdatedAt = DateTime.UtcNow;
            }
            existingTeam.PlayerCount = existingTeam.TeamPlayers.Count;
            await _context.SaveChangesAsync(cancellationToken);
            return await Get(existingTeam.Id, cancellationToken);
        }

        public async Task<ICollection<Team>> GetConditionalTeamRange(ConditionalQuery.TeamConditions config, CancellationToken cancellationToken)
        {
            return await _teams.GetEntities(true)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaginatedResult<Team>> GetPaginatedTeamRange(ConditionalQuery.TeamConditions config, uint page, uint pageSize, CancellationToken cancellationToken)
        {
            int intPage = (int)page;
            int intSize = (int)pageSize;
            int count = _teams
                .AsNoTracking()
                .FilterWith(config)
                .Count();

            var list = await _teams.GetEntities(true)
                .FilterWith(config)
                .SortWith(config.Sort)
                .Skip((intPage - 1) * intSize)
                .Take(intSize)
                .ToListAsync(cancellationToken);
            return new PaginatedResult<Team>
            {
                Page = intPage,
                PageSize = intSize,
                Total = count,
                PageCount = (int)Math.Ceiling((double)count / pageSize),
                List = list
            };
        }
    }
}
