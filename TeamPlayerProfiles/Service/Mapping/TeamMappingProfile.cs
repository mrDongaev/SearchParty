using AutoMapper;
using DataAccess.Entities;
using Library.Models;
using Library.Models.Enums;
using Service.Contracts.Player;
using Service.Contracts.Team;

namespace Service.Mapping
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamDto>()
                .ForMember(d => d.AvgMmr, m => m.MapFrom(new AvgMmrResolver()))
                .ForMember(d => d.PlayersInTeam, m => m.MapFrom(src => src.TeamPlayers));

            CreateMap<TeamPlayer, TeamPlayerDto.Read>()
                .ForMember(d => d.Position, m => m.MapFrom(src => (PositionName)src.PositionId));

            CreateMap<TeamPlayerDto.Write, TeamPlayer>()
                .ForMember(d => d.PositionId, m => m.MapFrom(src => (int)src.Position))
                .ForMember(d => d.UserId, m => m.Ignore())
                .ForMember(d => d.User, m => m.Ignore())
                .ForMember(d => d.Player, m => m.Ignore())
                .ForMember(d => d.Team, m => m.Ignore())
                .ForMember(d => d.TeamId, m => m.MapFrom(src => Guid.Empty))
                .ForMember(d => d.Position, m => m.Ignore());

            CreateMap<CreateTeamDto, Team>()
                .ForMember(d => d.TeamPlayers, m => m.MapFrom(src => src.PlayersInTeam))
                .ForMember(d => d.PlayerCount, m => m.MapFrom(src => src.PlayersInTeam.Count))
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.Players, m => m.Ignore())
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.UpdatedAt, m => m.Ignore())
                .ForMember(d => d.User, d => d.Ignore());

            CreateMap<UpdateTeamDto, Team>()
                .ForMember(d => d.TeamPlayers, m => m.Ignore())
                .ForMember(d => d.UserId, m => m.Ignore())
                .ForMember(d => d.User, m => m.Ignore())
                .ForMember(d => d.Players, m => m.Ignore())
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.UpdatedAt, m => m.Ignore())
                .ForMember(d => d.PlayerCount, m => m.Ignore());

            CreateMap<PaginatedResult<Team>, PaginatedResult<TeamDto>>();
        }

        private class AvgMmrResolver : IValueResolver<Team, TeamDto, uint>
        {
            public uint Resolve(Team source, TeamDto destination, uint destMember, ResolutionContext context)
            {
                return (uint)Math.Round(source.TeamPlayers.Select(tp => tp.Player.User).Average(u => u.Mmr));
            }
        }
    }
}
