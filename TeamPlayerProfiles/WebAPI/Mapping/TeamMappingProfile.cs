using AutoMapper;
using Common.Models;
using DataAccess.Repositories.Models;
using Service.Contracts.Team;
using WebAPI.Contracts.Board;
using WebAPI.Contracts.Team;

namespace WebAPI.Mapping
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<TeamDto, GetTeam.Response>();

            CreateMap<TeamPlayerService.Read, TeamPlayerApi.Response>();

            CreateMap<TeamPlayerApi.Request, TeamPlayerService.Write>();

            CreateMap<CreateTeam.Request, CreateTeamDto>();

            CreateMap<UpdateTeam.Request, UpdateTeamDto>();

            CreateMap<ConditionalProfile.TeamRequest, ConditionalQuery.TeamConditions>();

            CreateMap<PaginatedResult<TeamDto>, PaginatedResult<GetTeam.Response>>();
        }
    }
}
