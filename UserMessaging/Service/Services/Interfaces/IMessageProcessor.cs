using Service.Models;

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
