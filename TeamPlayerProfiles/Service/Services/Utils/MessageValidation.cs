using DataAccess.Entities;
using FluentResults;
using Library.Models.API.UserMessaging;
using Library.Results.Errors.Validation.Message;
using Library.Results.Errors.Validation.Team;

namespace Service.Services.Utils
{
    public static class MessageValidation
    {
        public static Result ValidateInvitation(Guid userId, ICollection<GetPlayerInvitation.Response>? messages, ICollection<TeamPlayer>? teamPlayers, ProfileMessageSubmitted message)
        {
            return Result.Merge(
                Result.FailIf(message.AcceptingUserId == userId, new SelfMessagingError("User cannot send an invitation to their own player profile")),
                Result.FailIf(messages?.SingleOrDefault(m => m.AcceptingPlayerId == message.AcceptorId && m.InvitingTeamId == message.SenderId) != null,
                    new PendingMessageExistsError("A pending invitation already exists for these team and player profiles")),
                ValidateTeamPosition(teamPlayers, (int)message.PositionName),
                ValidatePlayerUniqueness(teamPlayers, message.AcceptorId)
                );
        }

        public static Result ValidateApplication(Guid userId, ICollection<GetTeamApplication.Response>? messages, ICollection<TeamPlayer>? teamPlayers, ProfileMessageSubmitted message)
        {
            return Result.Merge(
                Result.FailIf(message.AcceptingUserId == userId, new SelfMessagingError("User cannot send an application to their own team profile")),
                Result.FailIf(messages?.SingleOrDefault(m => m.AcceptingTeamId == message.AcceptorId && m.ApplyingPlayerId == message.SenderId) != null,
                    new PendingMessageExistsError("A pending application already exists for these team and player profiles")),
                ValidateTeamPosition(teamPlayers, (int)message.PositionName),
                ValidatePlayerUniqueness(teamPlayers, message.AcceptorId)
                );
        }

        private static Result ValidateTeamPosition(ICollection<TeamPlayer>? teamPlayers, int positionId)
        {
            return Result.FailIf(teamPlayers?.SingleOrDefault(tp => tp.PositionId == positionId) != null, new TeamPositionOverlapError(positionId));
        }

        private static Result ValidatePlayerUniqueness(ICollection<TeamPlayer>? teamPlayers, Guid acceptorId)
        {
            return Result.FailIf(teamPlayers?.SingleOrDefault(tp => tp.PlayerId == acceptorId) != null, new TeamContainsPlayerError());
        }
    }
}
