using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces.HeroInterfaces;
using WebAPI.Models.Hero;

namespace WebAPI.Controllers.Hero
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class HeroController(IHeroService heroService, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetHero.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetHero.Response>, NotFound>> Get(int id, CancellationToken cancellationToken)
        {
            var hero = await heroService.Get(id, cancellationToken);
            return hero == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetHero.Response>(hero));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetHero.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetRange(ICollection<int> ids, CancellationToken cancellationToken)
        {
            var heroes = await heroService.GetRange(ids, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetHero.Response>>(heroes));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetHero.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var heroes = await heroService.GetAll(cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetHero.Response>>(heroes));
        }
    }
}
