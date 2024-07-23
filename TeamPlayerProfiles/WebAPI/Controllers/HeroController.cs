using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using WebAPI.Contracts.Hero;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HeroController(IHeroService heroService, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        public async Task<Results<Ok<GetHero.Response>, NotFound>> Get(int id, CancellationToken cancellationToken)
        {
            var hero = await heroService.Get(id, cancellationToken);
            return hero == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetHero.Response>(hero));
        }

        [HttpPost]
        public async Task<IResult> GetRange(ICollection<int> ids, CancellationToken cancellationToken)
        {
            var heroes = await heroService.GetRange(ids, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetHero.Response>>(heroes));
        }

        [HttpGet]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var heroes = await heroService.GetAll(cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetHero.Response>>(heroes));
        }
    }
}
