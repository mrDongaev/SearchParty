using AutoMapper;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class PlayerInvitationMappingProfile : Profile
    {
        public PlayerInvitationMappingProfile()
        {
            CreateMap<PlayerInvitationDto, GetPlayerInvitation.Response>();

            CreateMap<ActionResponse<PlayerInvitationDto>, GetActionResponse.Response<GetPlayerInvitation.Response>>();
        }
    }
}
