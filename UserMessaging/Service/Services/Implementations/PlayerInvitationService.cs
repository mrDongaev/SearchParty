using Library.Models.Enums;
using Service.Dtos;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces;

namespace Service.Services.Implementations
{
    public class PlayerInvitationService(IPlayerInvitationRepository playerInvitationRepo) : IPlayerInvitationService
    {
        public async Task<PlayerInvitationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            return await playerInvitationRepo.GetMessage(id, cancellationToken);
        }

        public async Task<ICollection<PlayerInvitationDto>> GetPendingUserMessages(Guid userId, CancellationToken cancellationToken)
        {
            return await playerInvitationRepo.GetUserMessages(userId, MessageStatus.Pending, cancellationToken);
        }
    }
}
