﻿namespace Service.Contracts.Team
{
    public class UpdateTeamDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public ISet<TeamPlayerDto.Write>? PlayersInTeam { get; set; } = null;
    }
}
