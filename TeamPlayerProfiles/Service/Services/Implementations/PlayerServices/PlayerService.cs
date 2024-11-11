using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Player;
using Service.Services.Interfaces.PlayerInterfaces;

namespace Service.Services.Implementations.PlayerServices
{
    public class PlayerService(IMapper mapper, IPlayerRepository playerRepo) : IPlayerService
    {
        public async Task<PlayerDto> Create(CreatePlayerDto dto, CancellationToken cancellationToken = default)
        {
            var newPlayer = mapper.Map<Player>(dto);
            var createdPlayer = await playerRepo.Add(newPlayer, cancellationToken);
            return mapper.Map<PlayerDto>(createdPlayer);
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return await playerRepo.Delete(id, cancellationToken);
        }

        public async Task<PlayerDto?> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var player = await playerRepo.Get(id, cancellationToken);
            return player == null ? null : mapper.Map<PlayerDto>(player);
        }

        public async Task<ICollection<PlayerDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<PlayerDto>>(players);
        }

        public async Task<Guid?> GetProfileUserId(Guid profileId, CancellationToken cancellationToken = default)
        {
            return await playerRepo.GetProfileUserId(profileId, cancellationToken);
        }

        public async Task<ICollection<PlayerDto>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetRange(ids, cancellationToken);
            return mapper.Map<ICollection<PlayerDto>>(players);
        }

        public async Task<PlayerDto?> Update(UpdatePlayerDto dto, CancellationToken cancellationToken = default)
        {
            var player = mapper.Map<Player>(dto);
            var updatedPlayer = await playerRepo.Update(player, dto.HeroIds, cancellationToken);
            return updatedPlayer == null ? null : mapper.Map<PlayerDto>(updatedPlayer);
        }
    }
}
