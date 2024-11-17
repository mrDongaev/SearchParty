using Service.Contracts.Player;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.PlayerInterfaces
{
    public interface IPlayerService : IProfileService<PlayerDto, UpdatePlayerDto, CreatePlayerDto>
    {
    }
}
