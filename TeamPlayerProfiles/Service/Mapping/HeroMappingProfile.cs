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
