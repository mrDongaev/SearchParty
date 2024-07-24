using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Player;
using Service.Services.Interfaces;

namespace Service.Services.Implementations
{
    public class PlayerService(IMapper mapper, IPlayerRepository playerRepo, IHeroRepository heroRepo) : IPlayerService
    {
        public async Task<PlayerDto> Create(CreatePlayerDto dto, CancellationToken cancellationToken = default)
        {
            var newPlayer = mapper.Map<Player>(dto);
            var heroes = await heroRepo.GetRange(dto.HeroIds, cancellationToken);
            foreach (var hero in heroes)
            {
                newPlayer.Heroes.Add(hero);
            }
            var createdPlayer = await playerRepo.Add(newPlayer, cancellationToken);
            return mapper.Map<PlayerDto>(createdPlayer);
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return await playerRepo.Delete(id, cancellationToken);
        }

        public async Task<PlayerDto> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var player = await playerRepo.Get(id, cancellationToken);
            return mapper.Map<PlayerDto>(player);
        }

        public async Task<ICollection<PlayerDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<PlayerDto>>(players);
        }

        public async Task<ICollection<PlayerDto>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetRange(ids, cancellationToken);
            return mapper.Map<ICollection<PlayerDto>>(players);
        }

        public async Task<PlayerDto> Update(UpdatePlayerDto dto, CancellationToken cancellationToken = default)
        {
            var existingPlayer = await playerRepo.Get(dto.Id, cancellationToken);
            if (existingPlayer == null)
            {
                return null;
            }
            var player = mapper.Map<Player>(dto);
            var heroes = await heroRepo.GetRange(dto.HeroIds, cancellationToken);
            foreach (var hero in heroes)
            {
                player.Heroes.Add(hero);
            }
            var updatedPlayer = await playerRepo.Update(player, cancellationToken);
            return mapper.Map<PlayerDto>(updatedPlayer);
        }
    }
}
