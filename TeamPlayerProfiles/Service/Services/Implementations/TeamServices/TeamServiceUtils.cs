using Common.Exceptions;
using DataAccess.Entities;

namespace Service.Services.Implementations.TeamServices
{
    public static class TeamServiceUtils
    {
        public static bool TeamOwnerPlayerIsInTeam(this Team team)
        {
            return team.TeamPlayers.SingleOrDefault(tp => tp.UserId == team.UserId) != null || team.Players.SingleOrDefault(p => p.UserId == team.UserId) != null;
        }

        public static bool TeamPositionHasNoOverlap(this Team team)
        {
            return team.TeamPlayers.Select(tp => tp.PositionId).ToHashSet().Count == team.TeamPlayers.Count;
        }

        public static bool TeamCountIsValid(this Team team, uint maxCount)
        {
            return team.TeamPlayers.Count <= maxCount;
        }

        public static void CheckTeamValidity(this Team team, uint maxCount)
        {
            if (!team.TeamOwnerPlayerIsInTeam())
            {
                throw new TeamOwnerNotPresentException();
            }
            if (!team.TeamPositionHasNoOverlap())
            {
                throw new TeamPositionOverlapException();
            }
            if (team.TeamCountIsValid(maxCount))
            {
                throw new TeamCountOverflowException(maxCount);
            }
        }
    }
}
