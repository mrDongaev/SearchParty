using Library.Repositories.Interfaces;
using Service.Contracts.Player;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.PlayerInterfaces
{
    public interface IPlayerService : IProfileService<PlayerDto, UpdatePlayerDto, CreatePlayerDto>, IRangeGettable<PlayerDto, Guid>
    {
        Task<PlayerDto?> UpdatePlayerHeroes(Guid id, ISet<int> heroIds, CancellationToken cancellationToken);
    }
}
