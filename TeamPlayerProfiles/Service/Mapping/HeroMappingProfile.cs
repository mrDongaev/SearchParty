using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Entities;
using Service.Contracts.Hero;

namespace Service.Mapping
{
    public class HeroMappingProfile : Profile
    {
        public HeroMappingProfile()
        {
            CreateMap<Hero, HeroDto>();
        }
    }
}
