using AutoMapper;
using Service.Contracts.Team;
using WebAPI.Contracts.Team;

namespace WebAPI.Mappings
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<TeamDto, GetTeam.Response>();

            CreateMap<CreateTeam.Request, CreateTeamDto>();

            CreateMap<UpdateTeam.Request, UpdateTeamDto>();
        }
    }
}
