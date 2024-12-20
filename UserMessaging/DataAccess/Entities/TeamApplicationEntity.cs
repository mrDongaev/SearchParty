﻿using Library.Models.Enums;

namespace DataAccess.Entities
{
    public class TeamApplicationEntity : IMessageEntity
    {
        public Guid Id { get; set; }

        public Guid ApplyingPlayerId { get; set; }

        public Guid SendingUserId { get; set; }

        public Guid AcceptingTeamId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public PositionName PositionName { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
