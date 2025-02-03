﻿using AutoMapper;
using Library.Controllers;
using Library.Models.HttpResponses;
using Library.Results.Errors.Authorization;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Player;
using Service.Services.Interfaces.PlayerInterfaces;
using WebAPI.Models.Player;

namespace WebAPI.Controllers.Player
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PlayerController(IPlayerService playerService, IUserHttpContext userContext, IMapper mapper) : WebApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType<HttpResponseBody<GetPlayer.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<
                Results<Ok<HttpResponseBody<GetPlayer.Response>>,
                NotFound<HttpResponseBody<GetPlayer.Response?>>,
                UnauthorizedHttpResult>>
            Get(Guid id, CancellationToken cancellationToken)
        {
            var result = await playerService.Get(id, cancellationToken);

            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<PlayerDto?, GetPlayer.Response?>(res => null));
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetPlayer.Response>));
        }

        [HttpPost]
        [ProducesResponseType<HttpResponseBody<IEnumerable<GetPlayer.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<IEnumerable<GetPlayer.Response>>>,
            NotFound<HttpResponseBody<IEnumerable<GetPlayer.Response>>>>>
            GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var result = await playerService.GetRange(ids, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<PlayerDto>, IEnumerable<GetPlayer.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetPlayer.Response>>));
        }

        [HttpGet]
        [ProducesResponseType<HttpResponseBody<IEnumerable<GetPlayer.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<IEnumerable<GetPlayer.Response>>>,
            NotFound<HttpResponseBody<IEnumerable<GetPlayer.Response>>>>>
            GetAll(CancellationToken cancellationToken)
        {
            var result = await playerService.GetAll(cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<PlayerDto>, IEnumerable<GetPlayer.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetPlayer.Response>>));
        }

        [HttpGet]
        [ProducesResponseType<HttpResponseBody<IEnumerable<GetPlayer.Response>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<IEnumerable<GetPlayer.Response>>>,
            NotFound<HttpResponseBody<IEnumerable<GetPlayer.Response>>>>>
            GetPlayersOfUser(CancellationToken cancellationToken)
        {
            var result = await playerService.GetProfilesByUserId(userContext.UserId, cancellationToken);

            if (result.IsFailed)
            {
                return TypedResults.NotFound(result.MapToHttpResponseBody<ICollection<PlayerDto>, IEnumerable<GetPlayer.Response>>(res => []));
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<IEnumerable<GetPlayer.Response>>));
        }

        [HttpPost]
        [ProducesResponseType<HttpResponseBody<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> Create(CreatePlayer.Request request, CancellationToken cancellationToken)
        {
            var result = await playerService.Create(mapper.Map<CreatePlayerDto>(request), cancellationToken);
            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetPlayer.Response>));
        }

        [HttpPost("{id}")]
        [ProducesResponseType<HttpResponseBody<GetPlayer.Response>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<GetPlayer.Response>>,
            NotFound<HttpResponseBody<GetPlayer.Response?>>,
            UnauthorizedHttpResult>>
            Update(Guid id, [FromBody] UpdatePlayer.Request request, CancellationToken cancellationToken)
        {
            var tempPlayer = mapper.Map<UpdatePlayerDto>(request);
            tempPlayer.Id = id;
            var result = await playerService.Update(tempPlayer, cancellationToken);

            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody<PlayerDto?, GetPlayer.Response?>(res => null));
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(mapper.Map<GetPlayer.Response>));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<HttpResponseBody<bool>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<HttpResponseBody<bool>>(StatusCodes.Status404NotFound)]
        public async Task<Results<
            Ok<HttpResponseBody<bool>>,
            NotFound<HttpResponseBody<bool>>,
            UnauthorizedHttpResult>>
            Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await playerService.Delete(id, cancellationToken);

            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    return TypedResults.Unauthorized();
                }
                else
                {
                    return TypedResults.NotFound(result.MapToHttpResponseBody(res => res.Value));
                }
            }

            return TypedResults.Ok(result.MapToHttpResponseBody(res => res.Value));
        }
    }
}
