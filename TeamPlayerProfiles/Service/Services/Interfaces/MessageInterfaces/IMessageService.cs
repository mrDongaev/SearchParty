using Library.Models.API.UserMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces.MessageInterfaces
{
    public interface IMessageService<TGetMessageResponse> where TGetMessageResponse : GetMessage.Response
    {
        Task<TGetMessageResponse?> Get(Guid id, CancellationToken cancellationToken);
    }
}
