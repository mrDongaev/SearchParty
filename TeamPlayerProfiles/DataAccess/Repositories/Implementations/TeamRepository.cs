using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class TeamRepository : Repository<Team, Guid>, ITeamRepository
    {
        private readonly DbSet<Team> _teams;
        private readonly DbSet<Player> _players;
        private readonly DbSet<Position> _positions;

        public TeamRepository(TeamPlayerProfilesContext context) : base(context)
        {
            _teams = _context.Set<Team>();
            _players = _context.Set<Player>();
            _positions = _context.Set<Position>();
        }

        public override async Task<ICollection<Team>> GetAll(CancellationToken cancellationToken)
        {
            return await _teams
                .AsNoTracking()
                .Include(t => t.TeamPlayers)
                .ThenInclude(p => p.Player)
                .ThenInclude(p => p.Position)
                .Include(t => t.TeamPlayers)
                .ThenInclude(tp => tp.Position)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Team> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _teams
                .AsNoTracking()
                .Include(t => t.TeamPlayers)
                .ThenInclude(p => p.Player)
                .ThenInclude(p => p.Position)
                .Include(t => t.TeamPlayers)
                .ThenInclude(tp => tp.Position)
                .SingleAsync(t => t.Id == id, cancellationToken);
        }

        public override async Task<Team> Add(Team team, CancellationToken cancellationToken)
        {
            team.Id = Guid.NewGuid();
            foreach (var pit in team.PlayersInTeam)
            {
                team.TeamPlayers.Add(new TeamPlayer()
                {
                    TeamId = team.Id,
                    PlayerId = pit.PlayerId,
                    PositionId = (int)pit.Position
                });
            }
            await base.Add(team, cancellationToken);
            return await Get(team.Id, cancellationToken);
        }

        public override async Task<Team> Update(Team team, CancellationToken cancellationToken)
        {
            var existingTeam = await _teams
                .Include(t => t.TeamPlayers)
                .SingleOrDefaultAsync(t => t.Id == team.Id, cancellationToken);
            if (existingTeam == null)
            {
                return null;
            }
            existingTeam.Name = team.Name;
            existingTeam.Description = team.Description;
            existingTeam.Displayed = team.Displayed;
            existingTeam.UpdatedAt = DateTime.UtcNow;
            var existingPlayerIds = existingTeam.TeamPlayers.Select(tp => tp.PlayerId).ToList();
            var updatedPlayerIds = team.PlayersInTeam.Select(tp => tp.PlayerId).ToList();
            var playerIdsToAdd = updatedPlayerIds.Except(existingPlayerIds).ToList();
            var playerIdsToRemove = existingPlayerIds.Except(updatedPlayerIds);
            if (playerIdsToRemove.Any())
            {
                var teamPlayersToRemove = existingTeam.TeamPlayers
                    .Where(tp => playerIdsToRemove.Contains(tp.PlayerId))
                    .ToList();
                foreach (var teamPlayer in teamPlayersToRemove)
                {
                    existingTeam.TeamPlayers.Remove(teamPlayer);
                }
                _context.ChangeTracker.DetectChanges();
            }
            if (playerIdsToAdd.Count != 0)
            {
                var teamPlayersToAdd = team.PlayersInTeam
                    .Where(tp => playerIdsToAdd.Contains(tp.PlayerId))
                    .ToList();
                foreach (var teamPlayer in teamPlayersToAdd)
                {
                    existingTeam.TeamPlayers.Add(new TeamPlayer()
                    {
                        TeamId = existingTeam.Id,
                        PlayerId = teamPlayer.PlayerId,
                        PositionId = (int)teamPlayer.Position
                    });
                }
                _context.ChangeTracker.DetectChanges();
            }
            await _context.SaveChangesAsync(cancellationToken);
            return await Get(team.Id, cancellationToken);
        }
    }
}
