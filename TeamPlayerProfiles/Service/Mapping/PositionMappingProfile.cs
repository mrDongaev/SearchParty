using AutoMapper;
using DataAccess.Entities;
using Service.Contracts.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
