using Library.Models.API.UserMessaging;
using Library.Models.Enums;

namespace Service.Services.Interfaces.MessageInterfaces
{
    public interface IMessageService<TGetMessageResponse> where TGetMessageResponse : GetMessage.Response
    {
        public string AccessToken { set; }

        public string RefreshToken { set; }

        Task<TGetMessageResponse?> Get(Guid id, CancellationToken cancellationToken);

        Task<ICollection<TGetMessageResponse>?> GetUserMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken);
    }
}
