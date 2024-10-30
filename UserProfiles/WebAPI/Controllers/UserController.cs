using AutoMapper;
using Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.User;
using Service.Services.Interfaces;
using WebAPI.Contracts.User;
using WebAPI.Validation;

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
            var user = await userService.Get(id, cancellationToken);
            return user == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetUser.Response>(user));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetUser.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var users = await userService.GetRange(ids, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetUser.Response>>(users));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetUser.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var users = await userService.GetAll(cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetUser.Response>>(users));
        }

        [HttpPost]
        [ProducesResponseType<GetUser.Response>(StatusCodes.Status200OK)]
        public async Task<IResult> Create(CreateUser.Request request, CancellationToken cancellationToken)
        {
            var createdUser = await userService.Create(mapper.Map<CreateUserDto>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<GetUser.Response>(createdUser));
        }

        [HttpPost("{id}")]
        [ProducesResponseType<GetUser.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetUser.Response>, NotFound>> Update(Guid id, UpdateUser.Request request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<UpdateUserDto>(request);
            user.Id = id;
            var updatedUser = await userService.Update(user, cancellationToken);
            return updatedUser == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetUser.Response>(updatedUser));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            return TypedResults.Ok(await userService.Delete(id, cancellationToken));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetUser.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<IEnumerable<GetUser.Response>>, BadRequest>> GetFiltered([MmrValuesValid] GetConditionalUser.Request request, CancellationToken cancellationToken)
        {
            var users = await userService.GetFiltered(mapper.Map<ConditionalUserQuery>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetUser.Response>>(users));
        }
    }
}
