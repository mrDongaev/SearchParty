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

            CreateMap<CreateUser.Request, CreateUserDto>()
                                .ForAllMembers(opts =>
                                {
                                    opts.AllowNull();
                                    opts.Condition((src, dest, srcMember) => srcMember != null);
                                });

            CreateMap<UpdateUser.Request, UpdateUserDto>()
                .ForMember(d => d.Id, m => m.Ignore())
                                .ForAllMembers(opts =>
                                {
                                    opts.AllowNull();
                                    opts.Condition((src, dest, srcMember) => srcMember != null);
                                });
        }
    }
}
