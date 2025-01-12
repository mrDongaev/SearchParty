using AutoMapper;
using DataAccess.Repositories.Interfaces;
using FluentResults;
using Library.Results.Errors;
using Library.Results.Errors.EntityRequest;
using Service.Contracts.Hero;
using Service.Services.Interfaces.HeroInterfaces;

namespace Service.Services.Implementations.HeroServices
{
    public class HeroService(IMapper mapper, IHeroRepository heroRepo) : IHeroService
    {
        public async Task<Result<HeroDto?>> Get(int id, CancellationToken cancellationToken = default)
        {
            var hero = await heroRepo.Get(id, cancellationToken);

            if (hero == null)
            {
                return Result.Fail<HeroDto?>(new EntityNotFoundError($"Hero with the given ID has not been found")).WithValue(null);
            }

            return Result.Ok(mapper.Map<HeroDto?>(hero));
        }

        public async Task<Result<ICollection<HeroDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var heroes = await heroRepo.GetAll(cancellationToken);

            if (heroes.Count == 0)
            {
                return Result.Fail<ICollection<HeroDto>>(new EntityListNotFoundError("No heroes have been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<HeroDto>>(heroes));
        }

        public async Task<Result<ICollection<HeroDto>>> GetRange(ICollection<int> ids, CancellationToken cancellationToken = default)
        {
            var heroes = await heroRepo.GetRange(ids, cancellationToken);

            if (heroes.Count == 0)
            {
                return Result.Fail<ICollection<HeroDto>>(new EntityRangeNotFoundError("Heroes with the given IDs have not been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<HeroDto>>(heroes));
        }
    }
}
