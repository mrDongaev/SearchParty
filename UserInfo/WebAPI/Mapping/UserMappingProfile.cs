using AutoMapper;
using Service.Contracts.User;
using WebAPI.Models.User;

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
        }
    }
}
