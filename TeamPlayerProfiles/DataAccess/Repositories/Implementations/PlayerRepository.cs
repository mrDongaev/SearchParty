﻿using Common.Models.Enums;
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
            _players = _context.Players;
            _heroes = _context.Heroes;
            _positions = _context.Positions;
        }

        public override async Task<ICollection<Player>> GetAll(CancellationToken cancellationToken)
        {
            return await _players
                .AsNoTracking()
                .Include(p => p.Heroes)
                .Include(p => p.Position)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Player?> Get(Guid id, CancellationToken cancellationToken)
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
            var heroIds = player.Heroes.Select(h => h.Id).ToList();
            var heroes = await _heroes
                .Where(h => heroIds.Contains(h.Id))
                .ToListAsync(cancellationToken);
            player.Heroes.Clear();
            foreach (var hero in heroes)
            {
                player.Heroes.Add(hero);
            }
            var position = await _positions.SingleOrDefaultAsync(p => p.Id == player.PositionId, cancellationToken);
            if (position == null)
            {
                position = await _positions.SingleOrDefaultAsync(p => p.Id == (int)PositionName.Carry, cancellationToken);
            }
            player.Position = position;
            var newPlayer = await base.Add(player, cancellationToken);
            return newPlayer;
        }

        public override async Task<Player?> Update(Player player, CancellationToken cancellationToken)
        {
            var existingPlayer = await _players
                .Include(p => p.Heroes)
                .Include(p => p.Position)
                .SingleOrDefaultAsync(p => p.Id == player.Id, cancellationToken);
            if (existingPlayer == null)
            {
                return null;
            }
            var position = await _positions.SingleOrDefaultAsync(p => p.Id == player.PositionId, cancellationToken);
            if (player.Name != null) existingPlayer.Name = player.Name;
            if (player.Description != null) existingPlayer.Description = player.Description;
            if (player.Displayed != null) existingPlayer.Displayed = player.Displayed;
            if (position != null) existingPlayer.Position = position;
            if (_context.Entry(existingPlayer).State == EntityState.Modified)
            {
                existingPlayer.UpdatedAt = DateTime.UtcNow;
            }
            var updatedPlayer = await base.Update(existingPlayer, cancellationToken);
            return updatedPlayer;
        }

        public async Task<Player?> UpdatePlayerHeroes(Guid id, ISet<int> heroIds, CancellationToken cancellationToken)
        {
            var existingPlayer = await _players
                .Include(p => p.Heroes)
                .Include(p => p.Position)
                .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (existingPlayer == null)
            {
                return null;
            }
            var existingHeroIds = existingPlayer.Heroes.Select(p => p.Id).ToList();
            var updatedHeroIds = heroIds;
            var heroIdsToAdd = updatedHeroIds.Except(existingHeroIds).ToList();
            var heroIdsToRemove = existingHeroIds.Except(updatedHeroIds);
            if (heroIdsToRemove.Any())
            {
                var heroesToRemove = await _heroes
                .Where(h => heroIdsToRemove.Contains(h.Id))
                    .ToListAsync(cancellationToken);
                foreach (var hero in heroesToRemove)
                {
                    existingPlayer.Heroes.Remove(hero);
                }
                _context.Entry(existingPlayer).State = EntityState.Modified;
            }
            if (heroIdsToAdd.Count != 0)
            {
                var heroesToAdd = await _heroes
                .Where(h => heroIdsToAdd.Contains(h.Id))
                    .ToListAsync(cancellationToken);
                foreach (var hero in heroesToAdd)
                {
                    existingPlayer.Heroes.Add(hero);
                }
                _context.Entry(existingPlayer).State = EntityState.Modified;
            }
            if (_context.Entry(existingPlayer).State == EntityState.Modified)
            {
                existingPlayer.UpdatedAt = DateTime.UtcNow;
            }
            var updatedPlayer = await base.Update(existingPlayer, cancellationToken);
            return updatedPlayer;
        }
    }
}
