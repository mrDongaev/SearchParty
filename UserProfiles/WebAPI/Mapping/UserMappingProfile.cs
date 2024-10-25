using AutoMapper;
using DataAccess.Entities;
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

            CreateMap<UpdateUser.Request, UpdateUserDto>();
        }
    }
}
