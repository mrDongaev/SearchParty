using AutoMapper;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces.MessageInteraction;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PlayerInvitationController(IPlayerInvitationInteractionService playerInvitationService, IUserHttpContext userContext, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetPlayerInvitation.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetPlayerInvitation.Response>, UnauthorizedHttpResult, NotFound>> Get(Guid id, CancellationToken cancellationToken)
        {
            var message = await playerInvitationService.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return TypedResults.NotFound();
            }
            if (message.AcceptingUserId != userContext.UserId || message.SendingUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            return TypedResults.Ok(mapper.Map<GetPlayerInvitation.Response>(message));
        }

        [HttpPost("{userId}")]
        [ProducesResponseType<IEnumerable<GetPlayerInvitation.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<IEnumerable<GetPlayerInvitation.Response>>, BadRequest, UnauthorizedHttpResult>> GetUserMessages(Guid userId, [FromBody] ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            if (userContext.UserId != userId)
            {
                return TypedResults.Unauthorized();
            }
            foreach (var status in messageStatuses)
            {
                if (!Enum.IsDefined(status))
                {
                    return TypedResults.BadRequest();
                }
            }
            var messages = await playerInvitationService.GetUserMessages(userId, messageStatuses, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayerInvitation.Response>>(messages));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<GetActionResponse.Response<GetPlayerInvitation.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetActionResponse.Response<GetPlayerInvitation.Response>>, UnauthorizedHttpResult, NotFound>> Accept(Guid id, CancellationToken cancellationToken)
        {
            var message = await playerInvitationService.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return TypedResults.NotFound();
            }
            if (message.AcceptingUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var response = playerInvitationService.Accept(id, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetActionResponse.Response<GetPlayerInvitation.Response>>(response));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<GetActionResponse.Response<GetPlayerInvitation.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetActionResponse.Response<GetPlayerInvitation.Response>>, UnauthorizedHttpResult, NotFound>> Reject(Guid id, CancellationToken cancellationToken)
        {
            var message = await playerInvitationService.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return TypedResults.NotFound();
            }
            if (message.AcceptingUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var response = playerInvitationService.Reject(id, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetActionResponse.Response<GetPlayerInvitation.Response>>(response));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<GetActionResponse.Response<GetPlayerInvitation.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<GetActionResponse.Response<GetPlayerInvitation.Response>>, UnauthorizedHttpResult, NotFound>> Rescind(Guid id, CancellationToken cancellationToken)
        {
            var message = await playerInvitationService.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return TypedResults.NotFound();
            }
            if (message.SendingUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var response = playerInvitationService.Rescind(id, cancellationToken);
            return TypedResults.Ok(mapper.Map<GetActionResponse.Response<GetPlayerInvitation.Response>>(response));
        }

    }
}
