using AutoMapper;
using DataAccess.Entities;
using Library.Models.API.UserProfiles.User;
using Service.Contracts.User;

namespace Service.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.Mmr, m => m.Ignore());

            CreateMap<CreateUserDto, User>()
                .ForMember(d => d.UpdatedAt, m => m.Ignore());

            CreateMap<UpdateUserDto, User>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.UpdatedAt, m => m.Ignore());

            CreateMap<CreateUserDto, CreateUser.Request>();

            CreateMap<UpdateUserDto, UpdateUser.Request>();
        }
    }
}
