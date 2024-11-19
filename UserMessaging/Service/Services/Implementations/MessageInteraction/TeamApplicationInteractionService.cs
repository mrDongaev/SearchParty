using Library.Models.Enums;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces.MessageInteraction;

namespace Service.Services.Implementations.MessageInteraction
{
    public class TeamApplicationInteractionService(ITeamApplicationRepository teamApplicationRepo) : ITeamApplicationInteractionService
    {
        public async Task<TeamApplicationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            return await teamApplicationRepo.GetMessage(id, cancellationToken);
        }

        public async Task<ICollection<TeamApplicationDto>> GetUserMessages(Guid userId, MessageStatus messageStatus, CancellationToken cancellationToken)
        {
            return await teamApplicationRepo.GetUserMessages(userId, messageStatus, cancellationToken);
        }

        public Task<ActionResponse<TeamApplicationDto>> Accept(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<TeamApplicationDto>> Reject(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<TeamApplicationDto>> Rescind(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
