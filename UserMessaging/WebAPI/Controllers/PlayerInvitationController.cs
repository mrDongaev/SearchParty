using AutoMapper;
using Library.Models.API.TeamPlayerProfiles.Hero;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PlayerInvitationController(IPlayerInvitationService playerInvitationService, IUserHttpContext userContext, IMapper mapper) : WebApiController
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

        [HttpGet("{userId}")]
        [ProducesResponseType<IEnumerable<GetPlayerInvitation.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<IEnumerable<GetPlayerInvitation.Response>>, UnauthorizedHttpResult>> GetPendingMessages(Guid userId, CancellationToken cancellationToken)
        {
            if (userContext.UserId != userId)
            {
                return TypedResults.Unauthorized();
            }
            var messages = await playerInvitationService.GetPendingUserMessages(userId, cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayerInvitation.Response>>(messages));
        }
    }
}
