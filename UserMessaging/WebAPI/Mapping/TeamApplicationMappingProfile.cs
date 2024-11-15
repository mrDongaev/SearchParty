using AutoMapper;
using Service.Dtos;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class TeamApplicationMappingProfile : Profile
    {
        public TeamApplicationMappingProfile()
        {
            CreateMap<GetTeamApplication.Response, TeamApplicationDto>();
        }
    }
}
