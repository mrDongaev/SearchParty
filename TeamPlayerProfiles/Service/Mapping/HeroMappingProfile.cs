﻿using AutoMapper;
using DataAccess.Entities;
using Service.Contracts.Hero;

namespace Service.Mapping
{
    public class HeroMappingProfile : Profile
    {
        public HeroMappingProfile()
        {
            CreateMap<Hero, HeroDto>();

            CreateMap<int, Hero>()
                .ForMember(d => d.Id, m => m.MapFrom(src => src))
                .ForMember(d => d.Name, m => m.Ignore())
                .ForMember(d => d.MainStat, m => m.Ignore())
                .ForMember(d => d.Players, m => m.Ignore())
                .ForMember(d => d.PlayerHeroes, m => m.Ignore());

        }
    }
}
