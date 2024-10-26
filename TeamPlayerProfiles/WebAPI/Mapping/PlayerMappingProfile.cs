using AutoMapper;
using Common.Models;
using Library.Models;
using Service.Contracts.Player;
using WebAPI.Contracts.Board;
using WebAPI.Contracts.Player;

namespace WebAPI.Mapping
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<PlayerDto, GetPlayer.Response>();

            CreateMap<CreatePlayer.Request, CreatePlayerDto>();

            CreateMap<UpdatePlayer.Request, UpdatePlayerDto>();

            CreateMap<ConditionalProfile.PlayerRequest, ConditionalProfileQuery.PlayerConditions>();

            CreateMap<PaginatedResult<PlayerDto>, PaginatedResult<GetPlayer.Response>>();
        }
    }
}
