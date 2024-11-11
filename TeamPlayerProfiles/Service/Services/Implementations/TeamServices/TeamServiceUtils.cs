using Common.Exceptions;
using DataAccess.Entities;
using Service.Contracts.Team;

namespace Service.Services.Implementations.TeamServices
{
    public static class TeamServiceUtils
    {
        public static bool TeamOwnerPlayerIsInTeam(ICollection<Player> players, Guid teamUserId)
        {
            return players.SingleOrDefault(tp => tp.UserId == teamUserId) != null;
        }

        public static bool TeamPositionHasNoOverlap(ICollection<TeamPlayerDto.Write> players)
        {
            return players.Select(p => p.Position).ToHashSet().Count == players.Count;
        }

        public static bool TeamCountIsValid(ICollection<Player> teamPlayers, int maxCount)
        {
            return teamPlayers.Count <= maxCount;
        }

        public static void CheckTeamValidity(ICollection<Player> players, ICollection<TeamPlayerDto.Write> teamPlayers, Guid teamUserId, int maxCount)
        {
            if (!TeamOwnerPlayerIsInTeam(players, teamUserId))
            {
                throw new TeamOwnerNotPresentException();
            }
            if (!TeamPositionHasNoOverlap(teamPlayers))
            {
                throw new TeamPositionOverlapException();
            }
            if (!TeamCountIsValid(players, maxCount))
            {
                throw new TeamCountOverflowException(maxCount);
            }
        }
    }
}
