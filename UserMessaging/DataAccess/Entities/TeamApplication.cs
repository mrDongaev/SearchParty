using DataAccess.Entities.Interfaces;
using Library.Entities.Interfaces;
using Library.Models.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class TeamApplication : IMessageEntity
    {
        public Guid Id { get; set; }

        public Guid ApplyingPlayerId { get; set; }

        public Guid SendingUserId { get; set; }

        public Guid AcceptingTeamId { get; set; }

        public Guid AcceptingUserId {  get; set; }

        public MessageStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
