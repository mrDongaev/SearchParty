﻿using AutoMapper;
using Service.Contracts.Position;
using WebAPI.Contracts.Position;

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