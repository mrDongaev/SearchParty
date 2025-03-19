using AutoMapper;
using Service.Dtos.Message;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class PlayerInvitationMappingProfile : Profile
    {
        public PlayerInvitationMappingProfile()
        {
            CreateMap<PlayerInvitationDto, GetPlayerInvitation.Response>();
        }
    }
}
