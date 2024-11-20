using Service.Dtos.Message;

namespace Service.Repositories.Interfaces
{
    public interface ITeamApplicationRepository : IMessageRepository<TeamApplicationDto>
    {
    }
}
