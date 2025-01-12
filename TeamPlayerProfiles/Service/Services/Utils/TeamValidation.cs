using DataAccess.Entities;
using FluentResults;
using Library.Results.Errors.Validation.Team;
using Service.Contracts.Team;

namespace Service.Services.Utils
{
    public static class TeamValidation
    {
        private static Result TeamOwnerPlayerIsInTeam(ICollection<Player> players, Guid teamUserId)
        {
            return Result.FailIf(players.Count != 0 && players.SingleOrDefault(tp => tp.UserId == teamUserId) != null, new TeamOwnerNotPresentError());
        }

        private static Result TeamPositionHasNoOverlap(ICollection<TeamPlayerDto.Write> players)
        {
            var overlappingPositions = players
                .GroupBy(tp => tp.Position)
                .Where(g => g.Count() > 1)
                .Select(g => (int)g.Key)
                .ToArray();
            return Result.FailIf(players.Select(p => p.Position).Distinct().Count() != players.Count, new TeamPositionOverlapError(overlappingPositions));
        }

        private static Result TeamCountIsInBounds(ICollection<Player> teamPlayers, int maxCount)
        {
            return Result.FailIf(teamPlayers.Count > maxCount, new TeamCountOverflowError(maxCount));
        }

        public static Result Validate(ICollection<Player> players, ICollection<TeamPlayerDto.Write> teamPlayers, Guid teamUserId, int maxCount)
        {
            return Result.Merge(
                TeamOwnerPlayerIsInTeam(players, teamUserId),
                TeamPositionHasNoOverlap(teamPlayers),
                TeamCountIsInBounds(players, maxCount)
                );
        }
    }
}
