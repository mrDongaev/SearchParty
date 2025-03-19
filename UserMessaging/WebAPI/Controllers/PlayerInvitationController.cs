using AutoMapper;
using Library.Controllers;
using Library.Models.Enums;
using Library.Models.HttpResponses;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos.Message;
using Service.Services.Interfaces.MessageInteraction;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PlayerInvitationController(IPlayerInvitationInteractionService playerInvitationService, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetPlayerInvitation.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetPlayerInvitation.Response>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<GetPlayerInvitation.Response?>>>>
            Get(Guid id, CancellationToken cancellationToken)
        {
            var result = await playerInvitationService.GetMessage(id, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<PlayerInvitationDto?, GetPlayerInvitation.Response?>(res => null));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }
            return TypedResults.Ok(mapper.Map<GetPlayerInvitation.Response>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayerInvitation.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetPlayerInvitation.Response>>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<IEnumerable<GetPlayerInvitation.Response>>>>>
            GetUserMessages([FromBody] ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            var result = await playerInvitationService.GetUserMessages(messageStatuses, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<PlayerInvitationDto>, IEnumerable<GetPlayerInvitation.Response>>(res => []));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }
            return TypedResults.Ok(mapper.Map<IEnumerable<GetPlayerInvitation.Response>>(result.Value));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<HttpResponseBody<GetPlayerInvitation.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetPlayerInvitation.Response>>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<GetPlayerInvitation.Response?>>>>
            Accept(Guid id, CancellationToken cancellationToken)
        {
            var result = await playerInvitationService.Accept(id, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<PlayerInvitationDto?, GetPlayerInvitation.Response?>(res => null));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetPlayerInvitation.Response>));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<HttpResponseBody<GetPlayerInvitation.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetPlayerInvitation.Response>>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<GetPlayerInvitation.Response?>>>>
            Reject(Guid id, CancellationToken cancellationToken)
        {
            var result = await playerInvitationService.Reject(id, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<PlayerInvitationDto?, GetPlayerInvitation.Response?>(res => null));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetPlayerInvitation.Response>));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<HttpResponseBody<GetPlayerInvitation.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetPlayerInvitation.Response>>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<GetPlayerInvitation.Response?>>>>
            Rescind(Guid id, CancellationToken cancellationToken)
        {
            var result = await playerInvitationService.Rescind(id, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<PlayerInvitationDto?, GetPlayerInvitation.Response?>(res => null));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetPlayerInvitation.Response>));
        }
    }
}
