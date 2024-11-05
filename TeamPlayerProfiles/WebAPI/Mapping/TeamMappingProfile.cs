using AutoMapper;
using Common.Models;
using Library.Models;
using Service.Contracts.Team;
using WebAPI.Models.Team;

namespace WebAPI.Mapping
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<TeamDto, GetTeam.Response>();

            CreateMap<TeamPlayerDto.Read, UpdateTeamPlayers.Response>();

            CreateMap<UpdateTeamPlayers.Request, TeamPlayerDto.Write>();

            CreateMap<CreateTeam.Request, CreateTeamDto>();

            CreateMap<UpdateTeam.Request, UpdateTeamDto>()
                .ForMember(d => d.Id, m => m.Ignore());

            CreateMap<GetConditionalTeam.Request, ConditionalTeamQuery>();

            CreateMap<PaginatedResult<TeamDto>, PaginatedResult<GetTeam.Response>>();
        }
    }
}
