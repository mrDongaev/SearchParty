using Common.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using DataAccess.Utils;
using Library.Models;
using Library.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class TeamRepository : Repository<TeamPlayerProfilesContext, Team, Guid>, ITeamRepository
    {
        private readonly DbSet<Team> _teams;
        private readonly DbSet<Player> _players;
        public TeamRepository(TeamPlayerProfilesContext context) : base(context)
        {
            _teams = _context.Teams;
            _players = _context.Players;
        }

        public override async Task<ICollection<Team>> GetAll(CancellationToken cancellationToken)
        {
            return await _teams.GetEntities(true)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Team?> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _teams.GetEntities(true)
                .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public override async Task<ICollection<Team>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            return await _teams.GetEntities(true)
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<ICollection<Team>> GetProfilesByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await _teams.GetEntities(true)
                    .Where(t => t.UserId == userId || t.TeamPlayers.AsQueryable().SingleOrDefault(tp => tp.UserId == userId) != null)
                    .ToListAsync(cancellationToken);
        }

        public async Task<Guid?> GetProfileUserId(Guid teamId, CancellationToken cancellationToken)
        {
            var team = await _teams.AsNoTracking().SingleOrDefaultAsync(t => t.Id == teamId, cancellationToken);
            return team?.UserId;
        }

        public override async Task<Team> Add(Team team, CancellationToken cancellationToken)
        {
            team.Id = Guid.NewGuid();
            var playerIds = team.TeamPlayers.Select(tp => tp.PlayerId).ToList();
            var players = await _players.AsNoTracking().Where(p => playerIds.Contains(p.Id)).ToListAsync(cancellationToken);
            foreach (var tp in team.TeamPlayers)
            {
                tp.TeamId = team.Id;
                tp.UserId = players.Single(p => p.Id == tp.PlayerId).UserId;
            }
            team.PlayerCount = team.TeamPlayers.Count;
            await base.Add(team, cancellationToken);
            return await Get(team.Id, cancellationToken);
        }

        public async Task<Team?> Update(Team team, ISet<TeamPlayer>? players, CancellationToken cancellationToken)
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
            if (players != null) UpdateTeamPlayers(existingTeam, players);
            if (_context.Entry(existingTeam).State == EntityState.Modified)
            {
                existingTeam.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync(cancellationToken);
            return await Get(team.Id, cancellationToken);
        }

        private void UpdateTeamPlayers(Team existingTeam, ISet<TeamPlayer> players)
        {
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
            var playerIds = existingTeam.TeamPlayers.Select(tp => tp.PlayerId).ToList();
            var finalPlayers = _players.AsNoTracking().Where(p => playerIds.Contains(p.Id)).ToList();
            foreach (var tp in existingTeam.TeamPlayers)
            {
                tp.UserId = finalPlayers.Single(p => p.Id == tp.PlayerId).UserId;
            }
            existingTeam.PlayerCount = existingTeam.TeamPlayers.Count;
        }

        public async Task<ICollection<Team>> GetConditionalTeamRange(ConditionalTeamQuery config, CancellationToken cancellationToken)
        {
            return await _teams.GetEntities(true)
                .FilterWith(config)
                .SortWith(config.SortConditions)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaginatedResult<Team>> GetPaginatedTeamRange(ConditionalTeamQuery config, uint page, uint pageSize, CancellationToken cancellationToken)
        {
            int intPage = (int)page;
            int intSize = (int)pageSize;
            var query = _teams.GetEntities(true)
                .AsNoTracking()
                .FilterWith(config);

            int count = query.Count();

            var list = await query
                .SortWith(config.SortConditions)
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

        public async Task<ICollection<TeamPlayer>?> GetTeamPlayers(Guid teamId, CancellationToken cancellationToken)
        {
            var team = await _teams.AsNoTracking()
                .Include(t => t.TeamPlayers)
                .SingleOrDefaultAsync(t => t.Id == teamId, cancellationToken);
            return team != null ? team.TeamPlayers : null;
        }
    }
}
