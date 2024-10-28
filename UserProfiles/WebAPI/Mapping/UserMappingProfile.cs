using AutoMapper;
using Library.Models.QueryConditions;
using Service.Contracts.User;
using WebAPI.Contracts.User;

namespace WebAPI.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, GetUser.Response>();

            CreateMap<CreateUser.Request, CreateUserDto>();

            CreateMap<UpdateUser.Request, UpdateUserDto>()
                .ForMember(d => d.Id, m => m.Ignore());

            CreateMap<ConditionalUser.Request, UserConditionsDto>()
                .ForMember(d => d.Sort, m => m.MapFrom(s => new SortCondition()
                {
                    SortBy = "Mmr",
                    SortDirection = s.SortDirection,
                }));
        }
    }
}
