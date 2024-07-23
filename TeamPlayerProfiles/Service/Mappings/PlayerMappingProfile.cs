using AutoMapper;
using DataAccess.Entities;
using Service.Contracts.Player;

namespace Service.Mapping
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<Player, PlayerDto>();

            CreateMap<CreatePlayerDto, Player>()
                .ForMember(d => d.PositionId, m => m.MapFrom(src => (int)src.Position))
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.Teams, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore());

            CreateMap<UpdatePlayerDto, Player>()
                .ForMember(d => d.UserId, m => m.Ignore())
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.PositionId, m => m.MapFrom(src => (int)src.Position))
                .ForMember(d => d.Teams, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore());
        }
    }
}
