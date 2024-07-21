using AutoMapper;
using Service.Contracts.Player;
using WebAPI.Contracts.Player;

namespace WebAPI.Mappings
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<PlayerDto, GetPlayer.Response>();

            CreateMap<CreatePlayer.Request, CreatePlayerDto>();

            CreateMap<UpdatePlayer.Request, UpdatePlayerDto>(); 
        }
    }
}
