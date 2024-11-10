﻿using AutoMapper;
using Common.Models;
using Library.Models;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Interfaces.UserInterfaces;
using WebAPI.Models.Player;
using WebAPI.Models.Team;

namespace WebAPI.Controllers.Team
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TeamBoardController(IMapper mapper, ITeamBoardService teamService, IUserHttpContext userContext, IUserIdentityService userIdentity) : WebApiController
    {
        [HttpPost("{teamId}/{displayed}")]
        [ProducesResponseType<GetTeam.Response>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<GetTeam.Response>, NotFound, UnauthorizedHttpResult>> SetDisplayed(Guid teamId, bool displayed, CancellationToken cancellationToken)
        {
            var userId = await userIdentity.GetTeamUserId(teamId, cancellationToken);
            if (userId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var updatedPlayer = await teamService.SetDisplayed(teamId, displayed, cancellationToken);
            return updatedPlayer == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<GetTeam.Response>(updatedPlayer));
        }

        [HttpPost("{teamId}/{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok, BadRequest, UnauthorizedHttpResult>> SendTeamApplicationRequest(Guid teamId, Guid playerId, CancellationToken cancellationToken)
        {
            var playerUserId = await userIdentity.GetPlayerUserId(teamId, cancellationToken);
            if (playerUserId != userContext.UserId)
            {
                return TypedResults.Unauthorized();
            }
            var teamUserId = await userIdentity.GetTeamUserId(teamId, cancellationToken);
            if (teamUserId == userContext.UserId)
            {
                return TypedResults.BadRequest();
            }
            var message = new Message()
            {
                SenderId = teamId,
                SendingUserId = userContext.UserId,
                AcceptorId = playerId,
                AcceptingUserId = userContext.UserId,
                MessageType = MessageType.PlayerApplication,
            };
            await teamService.SendTeamApplicationRequest(message, cancellationToken);
            return TypedResults.Ok();
        }

        [HttpPost]
        [ProducesResponseType<IEnumerable<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetFiltered(GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var teams = await teamService.GetFiltered(mapper.Map<ConditionalTeamQuery>(request), cancellationToken);
            return TypedResults.Ok(mapper.Map<IEnumerable<GetTeam.Response>>(teams));
        }

        [HttpPost("{pageSize}/{page}")]
        [ProducesResponseType<PaginatedResult<GetPlayer.Response>>(StatusCodes.Status200OK)]
        public async Task<IResult> GetPaginated(uint page, uint pageSize, GetConditionalTeam.Request request, CancellationToken cancellationToken)
        {
            var teams = await teamService.GetPaginated(mapper.Map<ConditionalTeamQuery>(request), page, pageSize, cancellationToken);
            return TypedResults.Ok(mapper.Map<PaginatedResult<GetTeam.Response>>(teams));
        }
    }
}
