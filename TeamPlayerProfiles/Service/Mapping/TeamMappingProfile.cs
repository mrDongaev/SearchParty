using AutoMapper;
using Common.Models;
using DataAccess.Entities;
using Service.Contracts.Team;

namespace Service.Mapping
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamDto>()
                .ForMember(d => d.PlayersInTeam, m => m.MapFrom(src => src.TeamPlayers));

            CreateMap<TeamPlayer, PlayerInTeam>()
                .ForMember(d => d.PlayerId, m => m.MapFrom(src => src.PlayerId))
                .ForMember(d => d.Position, m => m.MapFrom(src => src.Position.Name));

            CreateMap<CreateTeamDto, Team>()
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.PlayerCount, m => m.MapFrom(src => src.PlayersInTeam.Count))
                .ForMember(d => d.Players, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore());

            CreateMap<UpdateTeamDto, Team>()
                .ForMember(d => d.UserId, m => m.Ignore())
                .ForMember(d => d.PlayerCount, m => m.MapFrom(src => src.PlayersInTeam.Count))
                .ForMember(d => d.Players, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore());
        }
    }
}
