using DataAccess.Entities.Interfaces;
using Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class PlayerInvitation : IMessageEntity
    {
        public Guid Id { get; set; }

        public Guid InvitingTeamId { get; set; }

        public Guid SendingUserId {  get; set; }

        public Guid AcceptingPlayerId { get; set; }

        public Guid AcceptingUserId {  get; set; }

        public MessageStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
