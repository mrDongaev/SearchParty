using Library.Models.API.UserMessaging;
using Service.Models;

namespace Service.Services.Interfaces
{
    public interface IMessageProcessor
    {
        public IMessage CreateMessage(ProfileMessageSubmitted message);

        public void SaveMessage(IMessage message);

        public void CheckSignalRRecipients();

        public void DispatchMessageToRecipient();

        // в teamplayerprofiles acceptplayer и exitteam методы для взаимодейсвтия
    }
}
