using AutoMapper;
using FluentResults;
using Library.Controllers;
using Library.Models.HttpResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Hero;
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
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetHero.Response>,
            NotFound<HttpResponseBody<GetHero.Response?>>>>
            Get(int id, CancellationToken cancellationToken)
        {
            Result<HeroDto?> result = await heroService.Get(id, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<HeroDto?, GetHero.Response?>(res => null));
            }

            return TypedResults.Ok(mapper.Map<GetHero.Response>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetHero.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetHero.Response>>,
            NotFound<HttpResponseBody<IEnumerable<GetHero.Response>>>>>
            GetRange(ICollection<int> ids, CancellationToken cancellationToken)
        {
            var result = await heroService.GetRange(ids, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<HeroDto>, IEnumerable<GetHero.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetHero.Response>>(result.Value));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetHero.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetHero.Response>>,
            NotFound<HttpResponseBody<IEnumerable<GetHero.Response>>>>>
            GetAll(CancellationToken cancellationToken)
        {
            var result = await heroService.GetAll(cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<HeroDto>, IEnumerable<GetHero.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetHero.Response>>(result.Value));
        }
    }
}
