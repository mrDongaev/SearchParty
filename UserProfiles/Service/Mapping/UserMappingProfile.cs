using AutoMapper;
using DataAccess.Entities;
using Service.Contracts.User;

namespace Service.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<UserDto, User>()
                .ForMember(d => d.Id, m => m.Ignore());

            CreateMap<CreateUserDto, User>()
                .ForMember(d => d.Id, m => m.Ignore());

            CreateMap<UpdateUserDto, User>()
                .ForMember(d => d.Id, m => m.Ignore());
        }
    }
}
