using Library.Repositories.Interfaces;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IMessageProcessor
    {
        public IMessage CreateMessage(Guid SenderId, Guid SendingUserId, Guid AcceptorId, Guid AcceptingUserId);

        public void SaveMessage(IMessage message);

        public void SendMessageBySignalR();

        // в teamplayerprofiles acceptplayer и exitteam методы для взаимодейсвтия
    }
}
