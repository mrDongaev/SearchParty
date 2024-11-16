using Microsoft.AspNetCore.Http.HttpResults;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.States.Interfaces
{
    public abstract class AbstractMessageState<TMessage, TMessageDto>
        where TMessage : AbstractMessage<TMessageDto> 
        where TMessageDto : MessageDto
    {
        public virtual TMessage Message { get; set; }

        public AbstractMessageState(TMessage message)
        {
            Message = message;
        }

        public abstract Task<ActionResponse<TMessageDto>> Accept(CancellationToken cancellationToken);

        public abstract Task<ActionResponse<TMessageDto>> Reject(CancellationToken cancellationToken);

        public abstract Task<ActionResponse<TMessageDto>> Rescind(CancellationToken cancellationToken);
    }
}
