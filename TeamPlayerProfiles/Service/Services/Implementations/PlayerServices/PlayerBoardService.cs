using AutoMapper;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Player;
using Service.Services.Interfaces.Common;

namespace Service.Services.Implementations.PlayerServices
{
    public class PlayerBoardService(IMapper mapper, IPlayerRepository playerRepo) : IBoardService<PlayerDto>
    {
        public async Task<PlayerDto> SetDisplay(Guid id, bool displayed, CancellationToken cancellationToken)
        {
            var player = await playerRepo.Get(id, cancellationToken);
            player.Displayed = displayed;
            var updatedPlayer = await playerRepo.Update(player, cancellationToken);
            return mapper.Map<PlayerDto>(updatedPlayer);
        }
    }
}
