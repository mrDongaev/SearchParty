using Service.Contracts.Team;

namespace Service.Services.Interfaces
{
    internal interface ITeamService: IProfileService<TeamDto, UpdateTeamDto, CreateTeamDto>
    {
    }
}
