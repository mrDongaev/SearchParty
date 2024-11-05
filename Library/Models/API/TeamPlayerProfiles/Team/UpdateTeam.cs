namespace Library.Models.API.TeamPlayerProfiles.Team
{
    public static class UpdateTeam
    {
        public sealed class Request
        {
            public string? Name { get; set; }

            public string? Description { get; set; }
        }
    }
}
