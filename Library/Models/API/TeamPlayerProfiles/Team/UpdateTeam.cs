namespace Library.Models.API.TeamPlayerProfiles.Team
{
    public static class UpdateTeam
    {
        public sealed class Request
        {
            public Guid Id { get; set; }

            public string? Name { get; set; }

            public string? Description { get; set; }

            public ISet<UpdateTeamPlayer.Request>? PlayersInTeam { get; set; }
        }
    }
}
