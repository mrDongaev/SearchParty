using AutoMapper;
using Service.Contracts.Hero;
using WebAPI.Models.Hero;

namespace WebAPI.Mapping
{
    public class HeroMappingProfile : Profile
    {
        public HeroMappingProfile()
        {
            CreateMap<HeroDto, GetHero.Response>();
        }
    }
}
