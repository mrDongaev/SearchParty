using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class PlayerRepository(TeamPlayerProfilesContext context, IHeroRepository heroRepo) : ProfileRepository<Player>(context), IPlayerRepository
    {
        private readonly DbSet<Player> _players = context.Set<Player>();

        public override async Task<ICollection<Player>> GetAll(CancellationToken cancellationToken)
        {
            return await _players
                .AsNoTracking()
                .Include(p => p.Heroes)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Player> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _players
                .AsNoTracking()
                .Include(p => p.Heroes)
                .SingleAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<ICollection<Player>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            return await _players
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .ToListAsync(cancellationToken);
        }

        public override async Task<Player> Update(Player player, CancellationToken cancellationToken)
        {
            var existingPlayer = await _players
                .Include(p => p.Heroes)
                .SingleAsync(p => p.Id == player.Id, cancellationToken);
            if (existingPlayer == null)
            {
                throw new Exception($"Профиль игрока с Id = {player.Id} не найден");
            }
            existingPlayer.Name = player.Name;
            existingPlayer.Description = player.Description;
            existingPlayer.Displayed = player.Displayed;
            existingPlayer.Position = player.Position;
            existingPlayer.UpdatedAt = DateTime.UtcNow;
            var existingHeroIds = existingPlayer.Heroes.Select(p => p.Id).ToList();
            var updatedHeroIds = player.Heroes.Select(p => p.Id).ToList();
            var heroIdsToAdd = updatedHeroIds.Except(existingHeroIds).ToList();
            var heroIdsToRemove = existingHeroIds.Except(updatedHeroIds);
            if (heroIdsToRemove.Any())
            {
                var heroesToRemove = player.Heroes
                    .Where(p => heroIdsToRemove.Contains(p.Id))
                    .ToList();
                foreach (var hero in heroesToRemove)
                {
                    existingPlayer.Heroes.Remove(hero);
                }
                context.ChangeTracker.DetectChanges();
            }
            if (heroIdsToAdd.Count != 0)
            {
                var heroesToAdd = await heroRepo.GetRange(heroIdsToAdd, cancellationToken);
                foreach (var hero in heroesToAdd)
                {
                    existingPlayer.Heroes.Add(hero);
                }
                context.ChangeTracker.DetectChanges();
            }
            await context.SaveChangesAsync(cancellationToken);
            return existingPlayer;
        }
    }
}
