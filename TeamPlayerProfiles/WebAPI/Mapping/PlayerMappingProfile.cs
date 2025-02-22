using AutoMapper;
using Common.Models;
using Library.Models;
using Service.Contracts.Player;
using WebAPI.Models.Player;

namespace WebAPI.Mapping
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<PlayerDto, GetPlayer.Response>();

            CreateMap<CreatePlayer.Request, CreatePlayerDto>()
                                .ForAllMembers(opts =>
                                {
                                    opts.AllowNull();
                                    opts.Condition((src, dest, srcMember) => srcMember != null);
                                });

            CreateMap<UpdatePlayer.Request, UpdatePlayerDto>()
                                .ForAllMembers(opts =>
                                {
                                    opts.AllowNull();
                                    opts.Condition((src, dest, srcMember) => srcMember != null);
                                });

            CreateMap<GetConditionalPlayer.Request, ConditionalPlayerQuery>()
                                .ForAllMembers(opts =>
                                {
                                    opts.AllowNull();
                                    opts.Condition((src, dest, srcMember) => srcMember != null);
                                });

            CreateMap<PaginatedResult<PlayerDto>, PaginatedResult<GetPlayer.Response>>();
        }
    }
}
