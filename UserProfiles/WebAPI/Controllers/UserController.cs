using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.User;
using Service.Services.Interfaces;
using Service.Services.Interfaces.Common;
using WebAPI.Contracts.User;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController(IUserService userService, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetUser.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetUser.Response>, NotFound>> Get(Guid id, CancellationToken cancellationToken)
        {
            var player = await userService.Get(id, cancellationToken);
            return player == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetUser.Response>(player));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetUser.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var players = await userService.GetRange(ids, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetUser.Response>>(players));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetUser.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var players = await userService.GetAll(cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetUser.Response>>(players));
        }

        [HttpPost]
        [ProducesResponseType<GetUser.Response>(StatusCodes.Status200OK)]
        public async Task<IResult> Create(CreateUser.Request request, CancellationToken cancellationToken)
        {
            var createdPlayer = await userService.Create(mapper.Map<CreateUserDto>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<GetUser.Response>(createdPlayer));
        }

        [HttpPost]
        [ProducesResponseType<GetUser.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetUser.Response>, NotFound>> Update(UpdateUser.Request request, CancellationToken cancellationToken)
        {
            var updatedPlayer = await userService.Update(mapper.Map<UpdateUserDto>(request), cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetUser.Response>(updatedPlayer));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            return TypedResults.Ok(await userService.Delete(id, cancellationToken));
        }
    }
}
