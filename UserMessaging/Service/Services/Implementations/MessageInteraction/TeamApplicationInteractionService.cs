using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;
using Service.Services.Implementations.MessageManagement;
using Service.Services.Interfaces.MessageInteraction;

namespace Service.Services.Implementations.MessageInteraction
{
    public class TeamApplicationInteractionService(ITeamApplicationRepository teamApplicationRepo, IServiceProvider serviceProvider, IUserHttpContext userContext, TeamApplicationManager manager) : ITeamApplicationInteractionService
    {
        public async Task<TeamApplicationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            return await teamApplicationRepo.GetMessage(id, cancellationToken);
        }

        public async Task<ICollection<TeamApplicationDto>> GetUserMessages(Guid userId, ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            return await teamApplicationRepo.GetUserMessages(userId, messageStatuses, cancellationToken);
        }

        public async Task<ActionResponse<TeamApplicationDto>> Accept(Guid id, CancellationToken cancellationToken)
        {
            ActionResponse<TeamApplicationDto> response;
            var message = await manager.GetOrCreateMessage(id, userContext, cancellationToken);
            if (message != null)
            {
                return await message.Accept();
            }
            else
            {
                response = new ActionResponse<TeamApplicationDto>();
                response.ActionMessage = "The team application could not be accepted";
                response.Status = ActionResponseStatus.Failure;
            }
            return response;
        }

        public async Task<ActionResponse<TeamApplicationDto>> Reject(Guid id, CancellationToken cancellationToken)
        {
            ActionResponse<TeamApplicationDto> response;
            var message = await manager.GetOrCreateMessage(id, userContext, cancellationToken);
            if (message != null)
            {
                return await message.Reject();
            }
            else
            {
                response = new ActionResponse<TeamApplicationDto>();
                response.ActionMessage = "The team application could not be rejected";
                response.Status = ActionResponseStatus.Failure;
            }
            return response;
        }

        public async Task<ActionResponse<TeamApplicationDto>> Rescind(Guid id, CancellationToken cancellationToken)
        {
            ActionResponse<TeamApplicationDto> response;
            var message = await manager.GetOrCreateMessage(id, userContext, cancellationToken);
            if (message != null)
            {
                return await message.Rescind();
            }
            else
            {
                response = new ActionResponse<TeamApplicationDto>();
                response.ActionMessage = "The team application could not be rescinded";
                response.Status = ActionResponseStatus.Failure;
            }
            return response;
        }
    }
}
