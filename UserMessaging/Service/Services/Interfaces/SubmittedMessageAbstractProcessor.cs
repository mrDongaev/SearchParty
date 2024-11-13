using Library.Models.API.UserMessaging;
using Service.Models;
using Service.Repositories.Interfaces;

namespace Service.Services.Interfaces
{
    public abstract class SubmittedMessageAbstractProcessor(IMessageRepository messageRepository)
    {
        protected abstract IMessage CreateMessage(ProfileMessageSubmitted submittedMessage);

        public async Task ProcessSubmittedMessage(ProfileMessageSubmitted submittedMessage)
        {
            IMessage message = CreateMessage(submittedMessage);
            message = await message.SaveToDatabase(messageRepository);
            message.TrySendToUser();
        }
    }
}
