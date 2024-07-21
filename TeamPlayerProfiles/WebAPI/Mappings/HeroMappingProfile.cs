using AutoMapper;
using Service.Contracts.Hero;
using WebAPI.Contracts.Hero;

namespace WebAPI.Mappings
{
    public class HeroMappingProfile : Profile
    {
        public HeroMappingProfile()
        {
            CreateMap<HeroDto, GetHero.Response>();
        }
    }
}
