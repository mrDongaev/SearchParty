using Library.Entities.Interfaces;
using Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Interfaces
{
    public interface IMessageEntity : IEntity<Guid>, IUpdateable
    {
        public Guid SendingUserId {  get; set; }

        public Guid AcceptingUserId { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
