using Library.Models.API.UserMessaging;
using Service.Dtos.Message;
using Service.Models.Message;

namespace Service.Services.Interfaces.MessageProcessing
{
    public abstract class SubmittedMessageAbstractProcessor
    {
        protected abstract AbstractMessage<PlayerInvitationDto> CreateMessage(ProfileMessageSubmitted submittedMessage);

        public async Task ProcessSubmittedMessage(ProfileMessageSubmitted submittedMessage)
        {
            AbstractMessage<PlayerInvitationDto> message = CreateMessage(submittedMessage);
            var messageDto = await message.SaveToDatabase();
        }
    }
}
