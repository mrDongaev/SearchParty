using AutoMapper;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Implementations;
using Service.Services.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TeamApplication(ITeamApplicationService teamApplicationService, IUserHttpContext userContext, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetTeamApplication.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetTeamApplication.Response>, UnauthorizedHttpResult, NotFound>> Get(Guid id, CancellationToken cancellationToken)
        {
            var message = await teamApplicationService.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return TypedResults.NotFound();
            }
            if (message.AcceptingUserId != userContext.UserId || message.SendingUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            return TypedResults.Ok(mapper.Map<GetTeamApplication.Response>(message));
        }

        [HttpGet("{userId}")]
        [ProducesResponseType<IEnumerable<GetTeamApplication.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<IEnumerable<GetTeamApplication.Response>>, UnauthorizedHttpResult>> GetPendingMessages(Guid userId, CancellationToken cancellationToken)
        {
            if (userContext.UserId != userId)
            {
                return TypedResults.Unauthorized();
            }
            var messages = await teamApplicationService.GetPendingUserMessages(userId, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeamApplication.Response>>(messages));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<GetTeamApplication.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetActionResponse.Response<GetTeamApplication.Response>>, UnauthorizedHttpResult, NotFound>> Accept(Guid id, CancellationToken cancellationToken)
        {
            var message = await teamApplicationService.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return TypedResults.NotFound();
            }
            if (message.AcceptingUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var response = teamApplicationService.Accept(id, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetActionResponse.Response<GetTeamApplication.Response>>(response));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<GetTeamApplication.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetActionResponse.Response<GetTeamApplication.Response>>, UnauthorizedHttpResult, NotFound>> Reject(Guid id, CancellationToken cancellationToken)
        {
            var message = await teamApplicationService.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return TypedResults.NotFound();
            }
            if (message.AcceptingUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var response = teamApplicationService.Reject(id, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetActionResponse.Response<GetTeamApplication.Response>>(response));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<GetTeamApplication.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetActionResponse.Response<GetTeamApplication.Response>>, UnauthorizedHttpResult, NotFound>> Rescind(Guid id, CancellationToken cancellationToken)
        {
            var message = await teamApplicationService.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return TypedResults.NotFound();
            }
            if (message.SendingUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var response = teamApplicationService.Rescind(id, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetActionResponse.Response<GetTeamApplication.Response>>(response));
        }
    }
}
