﻿using Library.Models.Enums;

namespace DataAccess.Entities
{
    public class PlayerInvitationEntity : IMessageEntity
    {
        public Guid Id { get; set; }

        public Guid InvitingTeamId { get; set; }

        public Guid SendingUserId { get; set; }

        public Guid AcceptingPlayerId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public PositionName PositionName { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
