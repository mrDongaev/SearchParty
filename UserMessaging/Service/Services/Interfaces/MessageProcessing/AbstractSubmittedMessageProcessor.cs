using Library.Models.API.UserMessaging;
using Service.Domain.Message;
using Service.Dtos.Message;

namespace Service.Services.Interfaces.MessageProcessing
{
    public abstract class AbstractSubmittedMessageProcessor<TMessageDto> where TMessageDto : MessageDto
    {
        public abstract AbstractMessage<TMessageDto> CreateMessage(ProfileMessageSubmitted submittedMessage);

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
