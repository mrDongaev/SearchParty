namespace WebAPI.Models.Team
{
    public static class GetTeam
    {
        public sealed class Response
        {
            public Guid Id { get; set; }

            public Guid UserId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public uint AvgMmr { get; set; }

            public bool Displayed { get; set; }

            public DateTime UpdatedAt { get; set; }

            public ISet<UpdateTeamPlayer.Response> PlayersInTeam { get; set; } = new HashSet<UpdateTeamPlayer.Response>();
        }
    }
}
