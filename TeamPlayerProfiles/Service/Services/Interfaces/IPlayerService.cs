using Service.Contracts.Player;

namespace Service.Services.Interfaces
{
    public interface IPlayerService : IProfileService<PlayerDto, UpdatePlayerDto, CreatePlayerDto>, IRangeGettable<PlayerDto, Guid>
    {
    }
}
