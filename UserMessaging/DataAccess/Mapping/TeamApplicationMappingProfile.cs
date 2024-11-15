using AutoMapper;
using DataAccess.Entities;
using Service.Dtos;

namespace DataAccess.Mapping
{
    public class TeamApplicationMappingProfile : Profile
    {
        public TeamApplicationMappingProfile()
        {
            CreateMap<TeamApplicationEntity, TeamApplicationDto>()
                .ReverseMap();
        }
    }
}
