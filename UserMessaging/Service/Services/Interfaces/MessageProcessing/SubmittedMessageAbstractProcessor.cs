using Library.Models.API.UserMessaging;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Repositories.Interfaces;

namespace Service.Services.Interfaces.MessageProcessing
{
    public abstract class SubmittedMessageAbstractProcessor
    {
        protected abstract AbstractMessage<MessageDto> CreateMessage(ProfileMessageSubmitted submittedMessage);

        public async Task ProcessSubmittedMessage(ProfileMessageSubmitted submittedMessage)
        {
            AbstractMessage<MessageDto> message = CreateMessage(submittedMessage);
            var messageDto = await message.SaveToDatabase();
            message.TrySendToUser();
        }
    }
}
