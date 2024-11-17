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

            CreateMap<TeamPlayerDto.Read, UpdateTeamPlayer.Response>();

            CreateMap<UpdateTeamPlayer.Request, TeamPlayerDto.Write>()
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<CreateTeam.Request, CreateTeamDto>()
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<UpdateTeam.Request, UpdateTeamDto>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<GetConditionalTeam.Request, ConditionalTeamQuery>()
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });

            CreateMap<PaginatedResult<TeamDto>, PaginatedResult<GetTeam.Response>>();
        }
    }
}
