using AutoMapper;
using Common.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Models;
using Service.Contracts.Player;
using Service.Services.Interfaces.PlayerInterfaces;

namespace Service.Services.Implementations.PlayerServices
{
    public class PlayerBoardService(IMapper mapper, IPlayerRepository playerRepo) : IPlayerBoardService
    {
        public async Task<PlayerDto?> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken = default)
        {
            var player = new Player() { Id = id, Displayed = displayed };
            var updatedPlayer = await playerRepo.Update(player, cancellationToken);
            return updatedPlayer == null ? null : mapper.Map<PlayerDto>(updatedPlayer);
        }

        public async Task InvitePlayerToTeam(Guid playerId, Guid invitingTeamId, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }

        public async Task<ICollection<PlayerDto>> GetFiltered(ConditionalPlayerQuery query, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetConditionalPlayerRange(query, cancellationToken);
            return mapper.Map<ICollection<PlayerDto>>(players);
        }

        public async Task<PaginatedResult<PlayerDto>> GetPaginated(ConditionalPlayerQuery query, uint page, uint pageSize, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetPaginatedPlayerRange(query, page, pageSize, cancellationToken);
            return mapper.Map<PaginatedResult<PlayerDto>>(players);
        }
    }
}
