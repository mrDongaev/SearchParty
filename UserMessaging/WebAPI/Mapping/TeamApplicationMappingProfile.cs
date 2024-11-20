using AutoMapper;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class TeamApplicationMappingProfile : Profile
    {
        public TeamApplicationMappingProfile()
        {
            CreateMap<TeamApplicationDto, GetTeamApplication.Response>();

            CreateMap<ActionResponse<TeamApplicationDto>, GetActionResponse.Response<GetTeamApplication.Response>>();
        }
    }
}
