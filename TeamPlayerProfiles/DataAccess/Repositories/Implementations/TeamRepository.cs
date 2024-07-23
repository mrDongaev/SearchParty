using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class TeamRepository(TeamPlayerProfilesContext context, IPlayerRepository playerRepo) : ProfileRepository<Team>(context), ITeamRepository
    {
        private readonly DbSet<Team> _teams = context.Set<Team>();

        public override async Task<ICollection<Team>> GetAll(CancellationToken cancellationToken)
        {
            return await _teams
                .AsNoTracking()
                .Include(t => t.Players)
                .ThenInclude(p => p.Heroes)
                .Include(t => t.TeamPlayers)
                .ThenInclude(tp => tp.Position)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Team> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _teams
                .AsNoTracking()
                .Include(t => t.Players)
                .ThenInclude(p => p.Heroes)
                .Include(t => t.TeamPlayers)
                .ThenInclude(tp => tp.Position)
                .SingleAsync(t => t.Id == id, cancellationToken);
        }

        public override async Task<Team> Update(Team team, CancellationToken cancellationToken)
        {
            var existingTeam = await _teams
                .Include(t => t.Players)
                .ThenInclude(p => p.Heroes)
                .Include(t => t.TeamPlayers)
                .SingleAsync(t => t.Id == team.Id, cancellationToken);
            if (existingTeam == null)
            {
                throw new Exception($"Профиль команды с Id = {team.Id} не найден");
            }
            existingTeam.Name = team.Name;
            existingTeam.Description = team.Description;
            existingTeam.Displayed = team.Displayed;
            existingTeam.UpdatedAt = DateTime.UtcNow;
            var existingPlayerIds = existingTeam.TeamPlayers.Select(tp => tp.PlayerId).ToList();
            var updatedPlayerIds = team.TeamPlayers.Select(tp => tp.PlayerId).ToList();
            var playerIdsToAdd = updatedPlayerIds.Except(existingPlayerIds).ToList();
            var playerIdsToRemove = existingPlayerIds.Except(updatedPlayerIds);
            if (playerIdsToRemove.Any())
            {
                var playersToRemove = existingTeam.Players
                    .Where(p => playerIdsToRemove.Contains(p.Id))
                    .ToList();
                foreach (var player in playersToRemove)
                {
                    existingTeam.Players.Remove(player);
                }
                context.ChangeTracker.DetectChanges();
                var teamPlayersToRemove = existingTeam.TeamPlayers
                    .Where(tp => playerIdsToRemove.Contains(tp.PlayerId))
                    .ToList();
                foreach (var teamPlayer in teamPlayersToRemove)
                {
                    existingTeam.TeamPlayers.Remove(teamPlayer);
                }
            }
            if (playerIdsToAdd.Count != 0)
            {
                var playersToAdd = await playerRepo.GetRange(playerIdsToAdd, cancellationToken);
                foreach (var player in playersToAdd)
                {
                    existingTeam.Players.Add(player);
                }
                context.ChangeTracker.DetectChanges();
                var teamPlayersToAdd = team.TeamPlayers
                    .Where(tp => playerIdsToAdd.Contains(tp.PlayerId))
                    .ToList();
                foreach (var teamPlayer in teamPlayersToAdd)
                {
                    existingTeam.TeamPlayers.Add(new TeamPlayer()
                    {
                        TeamId = teamPlayer.TeamId,
                        PlayerId = teamPlayer.PlayerId,
                        Position = teamPlayer.Position,
                    });
                }
            }
            await context.SaveChangesAsync(cancellationToken);
            return existingTeam;
        }
    }
}
