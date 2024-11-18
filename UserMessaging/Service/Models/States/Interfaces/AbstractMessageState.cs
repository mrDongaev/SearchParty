using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;

namespace Service.Models.States.Interfaces
{
    public abstract class AbstractMessageState<TMessageDto> where TMessageDto : MessageDto
    {
        public virtual AbstractMessage<TMessageDto> Message { get; set; }

        public Guid Id
        {
            get => this.Message.Id;
        }

        public Guid SendingUserId
        {
            get => this.SendingUserId;
        }

        public Guid AcceptingUserId
        {
            get => this.AcceptingUserId;
        }

        public PositionName PositionName
        {
            get => this.Message.PositionName;
            set
            {
                this.Message.PositionName = value;
            }
        }

        public MessageStatus Status
        {
            get => this.Message.Status;
            set
            {
                this.Message.Status = value;
            }
        }

        public DateTime IssuedAt
        {
            get => this.Message.IssuedAt;
        }

        public DateTime ExpiresAt
        {
            get => this.Message.ExpiresAt;
        }

        public DateTime UpdatedAt
        {
            get => this.Message.UpdatedAt;
        }

        public CancellationToken CancellationToken
        {
            get => this.Message.CancellationToken;
        }

        public IServiceProvider ServiceProvider
        {
            get => this.Message.ServiceProvider;
        }

        public IUserHttpContext UserContext
        {
            get => this.Message.UserContext;
        }

        public TMessageDto MessageDto
        {
            get => (TMessageDto) this.Message.MessageDto;
        }

        public AbstractMessageState(AbstractMessage<TMessageDto> message)
        {
            Message = message;
        }

        public abstract Task<ActionResponse<TMessageDto>> Accept();

        public abstract Task<ActionResponse<TMessageDto>> Reject();

        public abstract Task<ActionResponse<TMessageDto>> Rescind();
    }
}
