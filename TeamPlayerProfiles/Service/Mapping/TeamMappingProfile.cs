using AutoMapper;
using Common.Models;
using Common.Models.Enums;
using DataAccess.Entities;
using Service.Contracts.Player;
using Service.Contracts.Team;

namespace Service.Mapping
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamDto>()
                .ForMember(d => d.PlayersInTeam, m => m.MapFrom(src => src.TeamPlayers));

            CreateMap<TeamPlayer, TeamPlayerService.Read>()
                .ForMember(d => d.Position, m => m.MapFrom(src => (PositionName)src.PositionId));

            CreateMap<TeamPlayerService.Write, TeamPlayer>()
                .ForMember(d => d.PositionId, m => m.MapFrom(src => (int)src.Position))
                .ForMember(d => d.Player, m => m.Ignore())
                .ForMember(d => d.TeamId, m => m.MapFrom(src => Guid.Empty))
                .ForMember(d => d.Position, m => m.Ignore());

            CreateMap<CreateTeamDto, Team>()
                .ForMember(d => d.TeamPlayers, m => m.MapFrom(src => src.PlayersInTeam))
                .ForMember(d => d.PlayerCount, m => m.MapFrom(src => src.PlayersInTeam.Count))
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.Players, m => m.Ignore());

            CreateMap<UpdateTeamDto, Team>()
                .ForMember(d => d.TeamPlayers, m => m.Ignore())
                .ForMember(d => d.UserId, m => m.Ignore())
                .ForMember(d => d.Players, m => m.Ignore());
        }
    }
}
