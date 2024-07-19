using Service.Contracts.Team;

namespace Service.Services.Interfaces
{
    public interface ITeamService : IProfileService<TeamDto, UpdateTeamDto, CreateTeamDto>
    {
    }
}
