using AutoMapper;
using Common.Exceptions;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Service.Contracts.Player;
using Service.Contracts.Team;
using Service.Services.Interfaces.MessageInterfaces;
using Service.Services.Interfaces.TeamInterfaces;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamService(IMapper mapper, ITeamRepository teamRepo, IPlayerRepository playerRepo, IServiceProvider serviceProvider) : ITeamService
    {
        public async Task<TeamDto> Create(CreateTeamDto dto, CancellationToken cancellationToken = default)
        {
            var team = mapper.Map<Team>(dto);
            var players = await playerRepo.GetRange(dto.PlayersInTeam.Select(pt => pt.PlayerId).ToList(), cancellationToken);
            TeamServiceUtils.CheckTeamValidity(players, dto.PlayersInTeam, team.UserId, 5);
            var createdTeam = await teamRepo.Add(team, cancellationToken);
            return mapper.Map<TeamDto>(createdTeam);
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return await teamRepo.Delete(id, cancellationToken);
        }

        public async Task<TeamDto?> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var team = await teamRepo.Get(id, cancellationToken);
            return team == null ? null : mapper.Map<TeamDto>(team);
        }

        public async Task<ICollection<TeamDto>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetRange(ids, cancellationToken);
            return mapper.Map<ICollection<TeamDto>>(teams);
        }

        public async Task<ICollection<TeamDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<TeamDto>>(teams);
        }

        public async Task<TeamDto?> Update(UpdateTeamDto dto, CancellationToken cancellationToken = default)
        {
            var team = mapper.Map<Team>(dto);
            ISet<TeamPlayer>? teamPlayers = null;
            if (dto.PlayersInTeam != null)
            {
                var players = await playerRepo.GetRange(dto.PlayersInTeam.Select(pt => pt.PlayerId).ToList(), cancellationToken);
                var existingTeam = await teamRepo.Get(dto.Id, cancellationToken);
                if (existingTeam == null) return null;
                TeamServiceUtils.CheckTeamValidity(players, dto.PlayersInTeam, existingTeam.UserId, 5);
                teamPlayers = mapper.Map<ISet<TeamPlayer>>(dto.PlayersInTeam);
            }
            var updatedTeam = await teamRepo.Update(team, teamPlayers, cancellationToken);
            return updatedTeam == null ? null : mapper.Map<TeamDto>(updatedTeam);
        }

        public async Task<Guid?> GetProfileUserId(Guid profileId, CancellationToken cancellationToken = default)
        {
            return await teamRepo.GetProfileUserId(profileId, cancellationToken);
        }

        public async Task PushPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid? messageId, MessageType? messageType, CancellationToken cancellationToken)
        {
            Team currentTeam = await teamRepo.Get(teamId, cancellationToken);
            Player pushedPlayer = await playerRepo.Get(playerId, cancellationToken);
            if (messageId != null && messageType != null)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    GetMessage.Response? message;
                    if (messageType == MessageType.PlayerInvitation)
                    {
                        var messageService = scope.ServiceProvider.GetRequiredService<IPlayerInvitationService>();
                        message = await messageService.Get(messageId.Value, cancellationToken);
                    }
                    else
                    {
                        var messageService = scope.ServiceProvider.GetRequiredService<ITeamApplicationService>();
                        message = await messageService.Get(messageId.Value, cancellationToken);
                    }
                    if (message == null || message.Status != MessageStatus.Pending)
                    {
                        throw new NoPendingMessageException(messageType.Value);
                    }
                }
            }
            else if (currentTeam.UserId != pushedPlayer.UserId)
            {
                throw new TeamOwnerNotPresentException();
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
            await Update(updatedTeam, cancellationToken);
        }

        public async Task PullPlayerFromTeam(Guid teamId, Guid playerId, CancellationToken cancellationToken)
        {
            Team currentTeam = await teamRepo.Get(teamId, cancellationToken);
            Player pushedPlayer = await playerRepo.Get(playerId, cancellationToken);
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
            await Update(updatedTeam, cancellationToken);
        }
    }
}
