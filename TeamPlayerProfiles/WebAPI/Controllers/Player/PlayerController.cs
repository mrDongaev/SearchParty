using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Player;
using Service.Services.Interfaces.PlayerInterfaces;
using WebAPI.Contracts.Player;

namespace WebAPI.Controllers.Player
{
    [Route("api/[controller]/[action]")]
    public class PlayerController(IPlayerService playerService, IMapper mapper) : WebApiController
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

        [HttpPost]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        public async Task<IResult> Create(CreatePlayer.Request request, CancellationToken cancellationToken)
        {
            var createdPlayer = await playerService.Create(mapper.Map<CreatePlayerDto>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<GetPlayer.Response>(createdPlayer));
        }

        [HttpPost]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetPlayer.Response>, NotFound>> Update(UpdatePlayer.Request request, CancellationToken cancellationToken)
        {
            var updatedPlayer = await playerService.Update(mapper.Map<UpdatePlayerDto>(request), cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPlayer.Response>(updatedPlayer));
        }

        [HttpPost("{id}")]
        [ProducesResponseType<GetPlayer.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetPlayer.Response>, NotFound>> UpdatePlayerHeroes(Guid id, [FromBody] ISet<int> heroIds, CancellationToken cancellationToken)
        {
            var updatedPlayer = await playerService.UpdatePlayerHeroes(id, heroIds, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetPlayer.Response>(updatedPlayer));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            return TypedResults.Ok(await playerService.Delete(id, cancellationToken));
        }
    }
}
