using AutoMapper;
using Library.Models.HttpResponses;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Player;
using Service.Services.Interfaces.PlayerInterfaces;
using System.Collections.Generic;
using WebAPI.Models.Player;

namespace WebAPI.Controllers.Player
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PlayerController(IPlayerService playerService, IUserHttpContext userContext, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<HttpResponseBody<GetPlayer.Response>>, NotFound<HttpResponseBody<GetPlayer.Response?>>>> Get(Guid id, CancellationToken cancellationToken)
        {
            var result = await playerService.Get(id, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<PlayerDto?, GetPlayer.Response?>(res => null));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetPlayer.Response>));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<HttpResponseBody<IEnumerable<GetPlayer.Response>>>, NotFound<HttpResponseBody<IEnumerable<GetPlayer.Response>>>>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var result = await playerService.GetRange(ids, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<PlayerDto>, IEnumerable<GetPlayer.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetPlayer.Response>>));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<HttpResponseBody<IEnumerable<GetPlayer.Response>>>, NotFound<HttpResponseBody<IEnumerable<GetPlayer.Response>>>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await playerService.GetAll(cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<PlayerDto>, IEnumerable<GetPlayer.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetPlayer.Response>>));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<HttpResponseBody<IEnumerable<GetPlayer.Response>>>, UnauthorizedHttpResult>> GetPlayersOfUser(CancellationToken cancellationToken)
        {
            var result = await playerService.GetProfilesByUserId(userContext.UserId, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayer.Response>>(result));
        }

        [HttpPost]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        public async Task<IResult> Create(CreatePlayer.Request request, CancellationToken cancellationToken)
        {
            var createdPlayer = await playerService.Create(mapper.Map<CreatePlayerDto>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<GetPlayer.Response>(createdPlayer));
        }

        [HttpPost("{id}")]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<GetPlayer.Response>, NotFound, UnauthorizedHttpResult>> Update(Guid id, [FromBody] UpdatePlayer.Request request, CancellationToken cancellationToken)
        {
            var userId = await userIdentity.GetPlayerUserId(id, cancellationToken);
            if (!userId.HasValue) return TypedResults.NotFound();
            if (userId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var tempPlayer = mapper.Map<UpdatePlayerDto>(request);
            tempPlayer.Id = id;
            var updatedPlayer = await playerService.Update(tempPlayer, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPlayer.Response>(updatedPlayer));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<bool>, UnauthorizedHttpResult, NotFound>> Delete(Guid id, CancellationToken cancellationToken)
        {
            var userId = await userIdentity.GetPlayerUserId(id, cancellationToken);
            if (!userId.HasValue) return TypedResults.NotFound();
            if (userId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            return TypedResults.Ok(await playerService.Delete(id, cancellationToken));
        }
    }
}
