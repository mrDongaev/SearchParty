using Library.Models.Enums;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces;

namespace Service.Services.Implementations
{
    public class TeamApplicationService(ITeamApplicationRepository teamApplicationRepo) : ITeamApplicationService
    {
        public async Task<TeamApplicationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            return await teamApplicationRepo.GetMessage(id, cancellationToken);
        }

        public async Task<ICollection<TeamApplicationDto>> GetPendingUserMessages(Guid userId, CancellationToken cancellationToken)
        {
            return await teamApplicationRepo.GetUserMessages(userId, MessageStatus.Pending, cancellationToken);
        }
    }
}
