using AutoMapper;
using DataAccess.Entities;
using Service.Contracts.Player;
using Enums = DataAccess.Entities.Enums;

namespace Service.Mapping
{
    public class PlayerMappingProfile : Profile
    {
        PlayerMappingProfile()
        {
            CreateMap<Player, PlayerDto>();

            CreateMap<CreatePlayerDto, Player>()
                .ForMember(d => d.Position, m => m.MapFrom(src => Enum.Parse<Enums.Position>(src.Position)))
                .ForMember(d => d.PositionId, m => m.MapFrom(src => (int)Enum.Parse<Enums.Position>(src.Position)))
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.Teams, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore());

            CreateMap<UpdatePlayerDto, Player>()
                .ForMember(d => d.UserId, m => m.Ignore())
                .ForMember(d => d.Position, m => m.MapFrom(src => Enum.Parse<Enums.Position>(src.Position)))
                .ForMember(d => d.PositionId, m => m.MapFrom(src => (int)Enum.Parse<Enums.Position>(src.Position)))
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.Teams, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore());
        }
    }
}
