using AutoMapper;
using DataAccess.Entities;
using Service.Contracts.Team;
using System;

namespace Service.Mapping
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamDto>()
                .ForMember(d => d.Players, m => m.Ignore());

            CreateMap<CreateTeamDto, Team>()
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.PlayerCount, m => m.MapFrom(src => src.Players.Count))
                .ForMember(d => d.Players, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore());

            CreateMap<UpdateTeamDto, Team>()
                .ForMember(d => d.UserId, m => m.Ignore())
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.PlayerCount, m => m.MapFrom(src => src.Players.Count))
                .ForMember(d => d.Players, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore());

        }
    }
}
