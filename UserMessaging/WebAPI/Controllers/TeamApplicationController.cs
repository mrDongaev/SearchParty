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
    public class TeamApplication(ITeamApplicationInteractionService teamApplicationService, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<GetTeamApplication.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<GetTeamApplication.Response>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<GetTeamApplication.Response?>>>>
            Get(Guid id, CancellationToken cancellationToken)
        {
            var result = await teamApplicationService.GetMessage(id, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<TeamApplicationDto?, GetTeamApplication.Response?>(res => null));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }
            return TypedResults.Ok(mapper.Map<GetTeamApplication.Response>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetTeamApplication.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<IEnumerable<GetTeamApplication.Response>>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<IEnumerable<GetTeamApplication.Response>>>>>
            GetUserMessages([FromBody] ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            var result = await teamApplicationService.GetUserMessages(messageStatuses, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<TeamApplicationDto>, IEnumerable<GetTeamApplication.Response>>(res => []));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeamApplication.Response>>(result.Value));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<HttpResponseBody<GetTeamApplication.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetTeamApplication.Response>>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<GetTeamApplication.Response?>>>>
            Accept(Guid id, CancellationToken cancellationToken)
        {
            var result = await teamApplicationService.Accept(id, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<TeamApplicationDto?, GetTeamApplication.Response?>(res => null));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetTeamApplication.Response>));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<HttpResponseBody<GetTeamApplication.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetTeamApplication.Response>>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<GetTeamApplication.Response?>>>>
            Reject(Guid id, CancellationToken cancellationToken)
        {
            var result = await teamApplicationService.Reject(id, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<TeamApplicationDto?, GetTeamApplication.Response?>(res => null));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetTeamApplication.Response>));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<HttpResponseBody<GetTeamApplication.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetTeamApplication.Response>>,
            UnauthorizedHttpResult,
            NotFound<HttpResponseBody<GetTeamApplication.Response?>>>>
            Rescind(Guid id, CancellationToken cancellationToken)
        {
            var result = await teamApplicationService.Rescind(id, cancellationToken);
            if (result.IsFailed)
            {
                if (result.HasError<EntityNotFoundError>())
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<TeamApplicationDto?, GetTeamApplication.Response?>(res => null));
                }
                else if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetTeamApplication.Response>));
        }
    }
}
