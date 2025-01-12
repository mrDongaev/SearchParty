using FluentResults;
using Library.Models.Enums;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.TeamInterfaces
{
    public interface ITeamService : IProfileService<TeamDto, UpdateTeamDto, CreateTeamDto>
    {
        Task<Result<TeamDto?>> PushPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid? messageId, MessageType? messageType, CancellationToken cancellationToken);

        Task<Result<TeamDto?>> PullPlayerFromTeam(Guid teamId, Guid playerId, CancellationToken cancellationToken);
    }
}
