using AutoMapper;
using DataAccess.Entities;
using Library.Models;
using Service.Contracts.Player;

namespace Service.Mapping
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<Player, PlayerDto>()
                .ForMember(d => d.Position, m => m.MapFrom(src => src.Position.Name));

            CreateMap<CreatePlayerDto, Player>()
                .ForMember(d => d.Heroes, m => m.MapFrom(src => src.HeroIds))
                .ForMember(d => d.PositionId, m => m.MapFrom(src => (int)src.Position))
                .ForMember(d => d.Position, m => m.Ignore())
                .ForMember(d => d.Teams, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore())
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.UpdatedAt, m => m.Ignore())
                .ForMember(d => d.PlayerHeroes, m => m.Ignore());

            CreateMap<UpdatePlayerDto, Player>()
                .ForMember(d => d.UserId, m => m.Ignore())
                .ForMember(d => d.Heroes, m => m.Ignore())
                .ForMember(d => d.PositionId, m => m.MapFrom(new UpdatePositionResolver()))
                .ForMember(d => d.Position, m => m.Ignore())
                .ForMember(d => d.Teams, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore())
                .ForMember(d => d.Displayed, m => m.Ignore())
                .ForMember(d => d.UpdatedAt, m => m.Ignore())
                .ForMember(d => d.PlayerHeroes, m => m.Ignore());

            CreateMap<PaginatedResult<Player>, PaginatedResult<PlayerDto>>();
        }

        private class UpdatePositionResolver : IValueResolver<UpdatePlayerDto, Player, int?>
        {
            public int? Resolve(UpdatePlayerDto source, Player destination, int? destMember, ResolutionContext context)
            {
                return source.Position == null ? null : (int?)source.Position;
            }
        }
    }
}
