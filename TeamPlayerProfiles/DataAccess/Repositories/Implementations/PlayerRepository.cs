using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class PlayerRepository : Repository<Player, Guid>, IPlayerRepository
    {
        private readonly DbSet<Player> _players;
        private readonly DbSet<Hero> _heroes;
        private readonly DbSet<Position> _positions;

        public PlayerRepository(TeamPlayerProfilesContext context) : base(context)
        {
            _players = _context.Set<Player>();
            _heroes = _context.Set<Hero>();
            _positions = _context.Set<Position>();
        }

        public override async Task<ICollection<Player>> GetAll(CancellationToken cancellationToken)
        {
            return await _players
                .AsNoTracking()
                .Include(p => p.Heroes)
                .Include(p => p.Position)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Player> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _players
                .AsNoTracking()
                .Include(p => p.Heroes)
                .Include(p => p.Position)
                .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<ICollection<Player>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            return await _players
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Heroes)
                .Include(p => p.Position)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Player> Add(Player player, CancellationToken cancellationToken)
        {
            var heroes = await _heroes
                .Where(h => player.HeroIds.Contains(h.Id))
                .ToListAsync();
            var position = await _positions.SingleAsync(p => p.Id == player.PositionId);
            foreach (var hero in heroes)
            {
                player.Heroes.Add(hero);
            }
            player.Position = position;
            var newPlayer = await base.Add(player, cancellationToken);
            return newPlayer;
        }

        public override async Task<Player> Update(Player player, CancellationToken cancellationToken)
        {
            var existingPlayer = await _players
                .Include(p => p.Heroes)
                .Include(p => p.Position)
                .SingleOrDefaultAsync(p => p.Id == player.Id, cancellationToken);
            if (existingPlayer == null)
            {
                return null;
            }
            var position = await _positions.SingleAsync(p => p.Id == player.PositionId);
            existingPlayer.Name = player.Name;
            existingPlayer.Description = player.Description;
            existingPlayer.Displayed = player.Displayed;
            existingPlayer.Position = position;
            existingPlayer.UpdatedAt = DateTime.UtcNow;
            var existingHeroIds = existingPlayer.Heroes.Select(p => p.Id).ToList();
            var updatedHeroIds = player.HeroIds;
            var heroIdsToAdd = updatedHeroIds.Except(existingHeroIds).ToList();
            var heroIdsToRemove = existingHeroIds.Except(updatedHeroIds);
            if (heroIdsToRemove.Any())
            {
                var heroesToRemove = await _heroes
                    .Where(h => heroIdsToRemove.Contains(h.Id))
                    .ToListAsync();
                foreach (var hero in heroesToRemove)
                {
                    existingPlayer.Heroes.Remove(hero);
                }
                _context.ChangeTracker.DetectChanges();
            }
            if (heroIdsToAdd.Count != 0)
            {
                var heroesToAdd = await _heroes
                    .Where(h => heroIdsToAdd.Contains(h.Id))
                    .ToListAsync();
                foreach (var hero in heroesToAdd)
                {
                    existingPlayer.Heroes.Add(hero);
                }
                _context.ChangeTracker.DetectChanges();
            }
            var updatedPlayer = await base.Update(existingPlayer, cancellationToken);
            return updatedPlayer;
        }
    }
}
