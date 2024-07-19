using AutoMapper;
using DataAccess.Entities;
using Service.Contracts.Position;

namespace Service.Mapping
{
    public class PositionMappingProfile : Profile
    {
        PositionMappingProfile()
        {
            CreateMap<Position, PositionDto>();
        }
    }
}
