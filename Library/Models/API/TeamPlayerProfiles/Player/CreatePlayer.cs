﻿using Library.Models.Enums;

namespace Library.Models.API.TeamPlayerProfiles.Player
{
    public static class CreatePlayer
    {
        public sealed class Request
        {
            public Guid? UserId { get; set; }

            public string? Name { get; set; }

            public string? Description { get; set; }

            public PositionName? Position { get; set; }

            public ISet<int>? HeroIds { get; set; }
        }
    }
}
