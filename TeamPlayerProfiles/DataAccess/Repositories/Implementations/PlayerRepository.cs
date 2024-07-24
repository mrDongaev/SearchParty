using Common.Models.Enums;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class PlayerRepository : ProfileRepository<Player>, IPlayerRepository
    {
        private readonly DbSet<Player> _players;
        private readonly IHeroRepository _heroRepo;

        public PlayerRepository(TeamPlayerProfilesContext context, IHeroRepository heroRepo) : base(context)
        {
            _players = _context.Set<Player>();
            _heroRepo = heroRepo;
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
                .SingleAsync(p => p.Id == id, cancellationToken);
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
            foreach (var hero in player.Heroes)
            {
                _context.Attach(hero);
            }
            //_context.ChangeTracker.DetectChanges();
            return await base.Add(player, cancellationToken);
        }

        public override async Task<Player> Update(Player player, CancellationToken cancellationToken)
        {
            var existingPlayer = await _players
                .Include(p => p.Heroes)
                .Include(p => p.Position)
                .SingleAsync(p => p.Id == player.Id, cancellationToken);
            if (existingPlayer == null)
            {
                return null;
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
                var heroesToRemove = existingPlayer.Heroes
                    .Where(p => heroIdsToRemove.Contains(p.Id))
                    .ToList();
                foreach (var hero in heroesToRemove)
                {
                    existingPlayer.Heroes.Remove(hero);
                }
                _context.ChangeTracker.DetectChanges();
            }
            if (heroIdsToAdd.Count != 0)
            {
                var heroesToAdd = await _heroRepo.GetRange(heroIdsToAdd, cancellationToken);
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
