using AutoMapper;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Hero;
using Service.Services.Interfaces;

namespace Service.Services.Implementations
{
    public class HeroService(IMapper mapper, IHeroRepository heroRepo) : IHeroService
    {
        public async Task<HeroDto> Get(int id, CancellationToken cancellationToken = default)
        {
            var hero = await heroRepo.Get(id, cancellationToken);
            return mapper.Map<HeroDto>(hero);
        }

        public async Task<ICollection<HeroDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var heroes = await heroRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<HeroDto>>(heroes);
        }

        public async Task<ICollection<HeroDto>> GetRange(ICollection<int> ids, CancellationToken cancellationToken = default)
        {
            var heroes = await heroRepo.GetRange(ids, cancellationToken);
            return mapper.Map<ICollection<HeroDto>>(heroes);
        }
    }
}
