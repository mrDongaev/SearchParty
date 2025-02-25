using AutoMapper;
using Library.Controllers;
using Library.Models.HttpResponses;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.User;
using Service.Services.Interfaces;
using WebAPI.Models.User;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class UserController(IUserService userService, IUserHttpContext userContext, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetUser.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetUser.Response>,
            NotFound<HttpResponseBody<GetUser.Response?>>>>
            Get(Guid id, CancellationToken cancellationToken)
        {
            var result = await userService.Get(id, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<UserDto?, GetUser.Response?>(res => null));
            }

            return TypedResults.Ok(mapper.Map<GetUser.Response>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetUser.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetUser.Response>>,
            NotFound<HttpResponseBody<IEnumerable<GetUser.Response>>>>>
            GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var result = await userService.GetRange(ids, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<UserDto>, IEnumerable<GetUser.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetUser.Response>>(result.Value));
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<GetUser.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetUser.Response>>,
            NotFound<HttpResponseBody<IEnumerable<GetUser.Response>>>>>
            GetAll(CancellationToken cancellationToken)
        {
            var result = await userService.GetAll(cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<UserDto>, IEnumerable<GetUser.Response>>(res => []));
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<GetUser.Response>>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<GetUser.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<
            Ok<GetUser.Response>,
            BadRequest<HttpResponseBody<GetUser.Response>>,
            UnauthorizedHttpResult>>
            Create(CreateUser.Request request, CancellationToken cancellationToken)
        {
            var result = await userService.Create(mapper.Map<CreateUserDto>(request), cancellationToken);

            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else if (result.HasError<EntityAlreadyExistsError>())
                {
                    return TypedResults.BadRequest(result.MapToHttpResponseBody<UserDto?, GetUser.Response?>(res => null));
                }
            }

            return TypedResults.Ok(mapper.Map<GetUser.Response>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<GetUser.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetUser.Response>,
            NotFound<HttpResponseBody<GetUser.Response>>,
            UnauthorizedHttpResult>>
            Update(UpdateUser.Request request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<UpdateUserDto>(request);
            var result = await userService.Update(user, cancellationToken);

            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<UserDto?, GetUser.Response?>(res => null));
                }
            }

            return TypedResults.Ok(mapper.Map<GetUser.Response>(result.Value));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<bool>,
            NotFound<HttpResponseBody<bool>>,
            UnauthorizedHttpResult>>
            Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await userService.Delete(id, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody(res => res.Value));
                }
            }

            return TypedResults.Ok(result.Value);
        }
    }
}
