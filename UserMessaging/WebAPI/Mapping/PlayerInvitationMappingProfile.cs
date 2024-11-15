using AutoMapper;
using Service.Dtos;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class PlayerInvitationMappingProfile : Profile
    {
        public PlayerInvitationMappingProfile()
        {
            CreateMap<GetPlayerInvitation.Response, PlayerInvitationDto>();
        }
    }
}
