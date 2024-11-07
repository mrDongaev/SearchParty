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

            CreateMap<CreateUserDto, User>()
                .ForMember(d => d.UpdatedAt, m => m.Ignore())
                .ForMember(d => d.Players, m => m.Ignore())
                .ForMember(d => d.Teams, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore())
                                .ForAllMembers(opts => {
                                    opts.AllowNull();
                                    opts.Condition((src, dest, srcMember) => srcMember != null);
                                });

            CreateMap<UpdateUserDto, User>()
                .ForMember(d => d.UpdatedAt, m => m.Ignore())
                .ForMember(d => d.Players, m => m.Ignore())
                .ForMember(d => d.Teams, m => m.Ignore())
                .ForMember(d => d.TeamPlayers, m => m.Ignore())
                                .ForAllMembers(opts => {
                                    opts.AllowNull();
                                    opts.Condition((src, dest, srcMember) => srcMember != null);
                                });
        }
    }
}
