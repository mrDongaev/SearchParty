using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public interface IMessage
    {
        public Guid SenderId { get; set; }

        public Guid SendingUserId { get; set; }

        public Guid AcceptorId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt {  get; set; }

        public Task<IMessage> Accept { get; set; }

        public Task<IMessage> Reject { get; set; }

        public Task<IMessage> Expire { get; set; }
    }
}
