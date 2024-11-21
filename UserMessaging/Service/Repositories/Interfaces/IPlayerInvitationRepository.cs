using Service.Dtos.Message;

namespace Service.Repositories.Interfaces
{
    public interface IPlayerInvitationRepository : IMessageRepository<PlayerInvitationDto>
    {
    }
}
