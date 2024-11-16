﻿using Library.Models.Enums;

namespace WebAPI.Models
{
    public static class GetTeamApplication
    {
        public sealed class Response : GetMessage.Response
        {
            public Guid ApplyingPlayerId { get; set; }

            public Guid AcceptingTeamId { get; set; }
        }
    }
}
