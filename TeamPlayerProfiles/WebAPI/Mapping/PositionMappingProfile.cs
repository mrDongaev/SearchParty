using AutoMapper;
using Service.Contracts.Position;
using WebAPI.Models.Position;

namespace WebAPI.Mapping
{
    public class PositionMappingProfile : Profile
    {
        public PositionMappingProfile()
        {
            CreateMap<PositionDto, GetPosition.Response>();
        }
    }
}
