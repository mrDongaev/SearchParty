using Library.Models.Enums;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces.MessageInteraction;

namespace Service.Services.Implementations.MessageInteraction
{
    public class PlayerInvitationInteractionService(IPlayerInvitationRepository playerInvitationRepo) : IPlayerInvitationInteractionService
    {
        public async Task<PlayerInvitationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            return await playerInvitationRepo.GetMessage(id, cancellationToken);
        }

        public async Task<ICollection<PlayerInvitationDto>> GetPendingUserMessages(Guid userId, CancellationToken cancellationToken)
        {
            return await playerInvitationRepo.GetUserMessages(userId, MessageStatus.Pending, cancellationToken);
        }

        public Task<ActionResponse<PlayerInvitationDto>> Accept(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<PlayerInvitationDto>> Reject(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<PlayerInvitationDto>> Rescind(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
