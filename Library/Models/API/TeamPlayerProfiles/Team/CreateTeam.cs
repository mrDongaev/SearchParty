namespace Library.Models.API.TeamPlayerProfiles.Team
{
    public static class CreateTeam
    {
        public sealed class Request
        {
            public Guid? UserId { get; set; }

            public string? Name { get; set; }

            public string? Description { get; set; }

            public ISet<UpdateTeamPlayer.Request>? PlayersInTeam { get; set; }
        }
    }
}
