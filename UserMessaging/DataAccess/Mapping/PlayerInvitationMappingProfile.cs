using AutoMapper;
using DataAccess.Entities;
using Service.Dtos.Message;

namespace DataAccess.Mapping
{
    public class PlayerInvitationMappingProfile : Profile
    {
        public PlayerInvitationMappingProfile()
        {
            CreateMap<PlayerInvitationEntity, PlayerInvitationDto>()
                .ReverseMap();
        }
    }
}
