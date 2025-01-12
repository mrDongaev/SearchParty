using AutoMapper;
using Common.Exceptions;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using FluentResults;
using Library.Constants;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Results.Errors.EntityRequest;
using Library.Results.Errors.Validation.Message;
using Service.Contracts.Team;
using Service.Services.Interfaces.MessageInterfaces;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Utils;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamService(IMapper mapper, ITeamRepository teamRepo, IPlayerRepository playerRepo, IPlayerInvitationService playerInvitationService, ITeamApplicationService teamApplicationService) : ITeamService
    {
        public async Task<Result<TeamDto?>> Create(CreateTeamDto dto, CancellationToken cancellationToken = default)
        {
            var team = mapper.Map<Team>(dto);
            var players = await playerRepo.GetRange(dto.PlayersInTeam.Select(pt => pt.PlayerId).ToList(), cancellationToken);
            var validationResult = TeamValidation.Validate(players, dto.PlayersInTeam, team.UserId, SearchPartyConstants.MaxCount);
            if (validationResult.IsFailed)
            {
                return validationResult.ToResult<TeamDto?>().WithValue(null);
            }
            var createdTeam = await teamRepo.Add(team, cancellationToken);
            return Result.Ok(mapper.Map<TeamDto?>(createdTeam));
        }

        public async Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await teamRepo.Delete(id, cancellationToken);

            if (result)
            {
                return Result.Ok(true);
            }

            return Result.Fail<bool>(new EntityNotFoundError("Team with the given ID has not been found")).WithValue(false);
        }

        public async Task<Result<TeamDto?>> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var team = await teamRepo.Get(id, cancellationToken);

            if (team == null)
            {
                return Result.Fail<TeamDto?>(new EntityNotFoundError("Team with the given ID has not been found")).WithValue(null);
            }

            return Result.Ok(mapper.Map<TeamDto?>(team));
        }

        public async Task<Result<ICollection<TeamDto>>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetRange(ids, cancellationToken);

            if (teams.Count == 0)
            {
                return Result.Fail<ICollection<TeamDto>>(new EntityRangeNotFoundError("Teams with the given IDs have not been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<TeamDto>>(teams));
        }

        public async Task<Result<ICollection<TeamDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetAll(cancellationToken);

            if (teams.Count == 0)
            {
                return Result.Fail<ICollection<TeamDto>>(new EntityListNotFoundError("No teams have been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<TeamDto>>(teams));
        }

        public async Task<Result<ICollection<TeamDto>>> GetProfilesByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var teams = await teamRepo.GetProfilesByUserId(userId, cancellationToken);

            if (teams.Count == 0)
            {
                return Result.Fail<ICollection<TeamDto>>(new EntityRangeNotFoundError("No teams of user with the given ID have been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<TeamDto>>(teams));
        }

        public async Task<Result<TeamDto?>> Update(UpdateTeamDto dto, CancellationToken cancellationToken = default)
        {
            var team = mapper.Map<Team>(dto);
            ISet<TeamPlayer>? teamPlayers = null;
            if (dto.PlayersInTeam != null)
            {
                var players = await playerRepo.GetRange(dto.PlayersInTeam.Select(pt => pt.PlayerId).ToList(), cancellationToken);
                var existingTeam = await teamRepo.Get(dto.Id, cancellationToken);
                if (existingTeam == null) return Result.Fail<TeamDto?>(new EntityNotFoundError("Team with the given ID has not been found")).WithValue(null);

                var validationResult = TeamValidation.Validate(players, dto.PlayersInTeam, existingTeam.UserId, 5);
                if (validationResult.IsFailed)
                {
                    return validationResult.ToResult<TeamDto?>().WithValue(null);
                }

                teamPlayers = mapper.Map<ISet<TeamPlayer>>(dto.PlayersInTeam);
            }
            var updatedTeam = await teamRepo.Update(team, teamPlayers, cancellationToken);

            if (updatedTeam == null)
            {
                return Result.Fail<TeamDto?>(new EntityNotFoundError("Team with the given ID has not been found")).WithValue(null);
            }

            return Result.Ok(mapper.Map<TeamDto?>(updatedTeam));
        }

        public async Task<Result<Guid?>> GetProfileUserId(Guid profileId, CancellationToken cancellationToken = default)
        {
            var userId = await teamRepo.GetProfileUserId(profileId, cancellationToken);

            if (userId.HasValue)
            {
                return Result.Ok(userId);
            }

            return Result.Fail<Guid?>(new EntityNotFoundError("No users corresponding to the given team ID have been found")).WithValue(null);
        }

        public async Task<Result<TeamDto?>> PushPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid? messageId, MessageType? messageType, CancellationToken cancellationToken)
        {
            Team? currentTeam = await teamRepo.Get(teamId, cancellationToken);
            if (currentTeam == null)
            {
                return Result.Fail<TeamDto?>(new EntityNotFoundError("Team with the given ID has not been found")).WithValue(null);
            }
            Player? pushedPlayer = await playerRepo.Get(playerId, cancellationToken);
            if (pushedPlayer == null)
            {
                return Result.Fail<TeamDto?>(new EntityNotFoundError("Player with the given ID has not been found")).WithValue(null);
            }

            if (messageId != null && messageType != null)
            {
                GetMessage.Response? message;
                if (messageType == MessageType.PlayerInvitation)
                {
                    message = await playerInvitationService.Get(messageId.Value, cancellationToken);
                }
                else
                {
                    message = await teamApplicationService.Get(messageId.Value, cancellationToken);
                }

                if (message == null || message.Status != MessageStatus.Pending)
                {
                    return Result.Fail<TeamDto?>(new NoPendingMessageError(messageType.Value)).WithValue(null);
                }
            }

            UpdateTeamDto updatedTeam = new UpdateTeamDto
            {
                Id = teamId,
                PlayersInTeam = currentTeam.TeamPlayers.Select(tp => new TeamPlayerDto.Write
                {
                    PlayerId = tp.PlayerId,
                    Position = (PositionName)tp.PositionId,
                }).ToHashSet(),
            };
            updatedTeam.PlayersInTeam.Add(new TeamPlayerDto.Write
            {
                PlayerId = playerId,
                Position = position,
            });

            return await Update(updatedTeam, cancellationToken);
        }

        public async Task<Result<TeamDto?>> PullPlayerFromTeam(Guid teamId, Guid playerId, CancellationToken cancellationToken)
        {
            Team? currentTeam = await teamRepo.Get(teamId, cancellationToken);
            if (currentTeam == null)
            {
                return Result.Fail<TeamDto?>(new EntityNotFoundError("Team with the given ID has not been found")).WithValue(null);
            }
            Player? pushedPlayer = await playerRepo.Get(playerId, cancellationToken);
            if (pushedPlayer == null)
            {
                return Result.Fail<TeamDto?>(new EntityNotFoundError("Player with the given ID has not been found")).WithValue(null);
            }

            UpdateTeamDto updatedTeam = new UpdateTeamDto
            {
                Id = teamId,
                PlayersInTeam = currentTeam.TeamPlayers.Select(tp => new TeamPlayerDto.Write
                {
                    PlayerId = tp.PlayerId,
                    Position = (PositionName)tp.PositionId,
                }).ToHashSet(),
            };
            var playerToRemove = currentTeam.TeamPlayers.SingleOrDefault(currentTeam => currentTeam.PlayerId == playerId);
            updatedTeam.PlayersInTeam.Remove(new TeamPlayerDto.Write
            {
                PlayerId = playerId,
                Position = (PositionName)playerToRemove.PositionId,
            });

            return await Update(updatedTeam, cancellationToken);
        }
    }
}
