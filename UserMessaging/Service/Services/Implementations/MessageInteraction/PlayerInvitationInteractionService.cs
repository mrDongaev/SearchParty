using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;
using Service.Services.Implementations.MessageManagement;
using Service.Services.Interfaces.MessageInteraction;

namespace Service.Services.Implementations.MessageInteraction
{
    public class PlayerInvitationInteractionService(IPlayerInvitationRepository playerInvitationRepo, IServiceProvider serviceProvider, IUserHttpContext userContext, PlayerInvitationManager manager) : IPlayerInvitationInteractionService
    {
        public async Task<PlayerInvitationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            return await playerInvitationRepo.GetMessage(id, cancellationToken);
        }

        public async Task<ICollection<PlayerInvitationDto>> GetUserMessages(Guid userId, ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            return await playerInvitationRepo.GetUserMessages(userId, messageStatuses, cancellationToken);
        }

        public async Task<ActionResponse<PlayerInvitationDto>> Accept(Guid id, CancellationToken cancellationToken)
        {
            ActionResponse<PlayerInvitationDto> response;
            var message = await manager.GetOrCreateMessage(id, serviceProvider, userContext, cancellationToken);
            if (message != null)
            {
                return await message.Accept();
            }
            else
            {
                response = new ActionResponse<PlayerInvitationDto>();
                response.ActionMessage = "The player invitation could not be accepted - message not found";
                response.Status = ActionResponseStatus.Failure;
            }
            return response;
        }

        public async Task<ActionResponse<PlayerInvitationDto>> Reject(Guid id, CancellationToken cancellationToken)
        {
            ActionResponse<PlayerInvitationDto> response;
            var message = await manager.GetOrCreateMessage(id, serviceProvider, userContext, cancellationToken);
            if (message != null)
            {
                return await message.Reject();
            }
            else
            {
                response = new ActionResponse<PlayerInvitationDto>();
                response.ActionMessage = "The player invitation could not be rejected - message not found";
                response.Status = ActionResponseStatus.Failure;
            }
            return response;
        }

        public async Task<ActionResponse<PlayerInvitationDto>> Rescind(Guid id, CancellationToken cancellationToken)
        {
            ActionResponse<PlayerInvitationDto> response;
            var message = await manager.GetOrCreateMessage(id, serviceProvider, userContext, cancellationToken);
            if (message != null)
            {
                return await message.Rescind();
            }
            else
            {
                response = new ActionResponse<PlayerInvitationDto>();
                response.ActionMessage = "The player invitation could not be rescinded - message not found";
                response.Status = ActionResponseStatus.Failure;
            }
            return response;
        }
    }
}
