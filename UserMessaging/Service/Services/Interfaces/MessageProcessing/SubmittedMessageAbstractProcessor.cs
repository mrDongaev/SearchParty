using Library.Models.API.UserMessaging;
using Service.Dtos.Message;
using Service.Models.Message;

namespace Service.Services.Interfaces.MessageProcessing
{
    public abstract class SubmittedMessageAbstractProcessor<TMessageDto> where TMessageDto : MessageDto
    {
        protected abstract AbstractMessage<TMessageDto> CreateMessage(ProfileMessageSubmitted submittedMessage);

        public async Task ProcessSubmittedMessage(ProfileMessageSubmitted submittedMessage)
        {
            AbstractMessage<TMessageDto> message = CreateMessage(submittedMessage);
            var messageDto = await message.SaveToDatabase();
            if (messageDto != null)
            {
                message.TrySendToUser();
            }
        }
    }
}
