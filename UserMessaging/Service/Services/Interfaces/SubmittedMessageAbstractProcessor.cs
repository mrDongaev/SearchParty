using Library.Models.API.UserMessaging;
using Service.Dtos;
using Service.Models;
using Service.Repositories.Interfaces;

namespace Service.Services.Interfaces
{
    public abstract class SubmittedMessageAbstractProcessor
    {
        protected abstract Message<MessageDto> CreateMessage(ProfileMessageSubmitted submittedMessage);

        public async Task ProcessSubmittedMessage(ProfileMessageSubmitted submittedMessage)
        {
            Message<MessageDto> message = CreateMessage(submittedMessage);
            var messageDto = await message.SaveToDatabase();
            message.TrySendToUser();
        }
    }
}
