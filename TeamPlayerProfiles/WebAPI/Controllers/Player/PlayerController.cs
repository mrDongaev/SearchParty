using AutoMapper;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Player;
using Service.Services.Implementations.TeamServices;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.UserInterfaces;
using WebAPI.Models.Player;

namespace WebAPI.Controllers.Player
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PlayerController(IPlayerService playerService, IUserIdentityService userIdentity, IUserHttpContext userContext, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetPlayer.Response>, NotFound>> Get(Guid id, CancellationToken cancellationToken)
        {
            var player = await playerService.Get(id, cancellationToken);
            return player == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPlayer.Response>(player));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var players = await playerService.GetRange(ids, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayer.Response>>(players));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var players = await playerService.GetAll(cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayer.Response>>(players));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<IEnumerable<GetPlayer.Response>>, UnauthorizedHttpResult>> GetPlayersOfUser(CancellationToken cancellationToken)
        {
            var players = await playerService.GetProfilesByUserId(userContext.UserId, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayer.Response>>(players));
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
