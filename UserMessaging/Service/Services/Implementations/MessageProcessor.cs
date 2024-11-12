using Library.Models.API.UserMessaging;
using Service.Models;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Implementations
{
    public class MessageProcessor : IMessageProcessor
    {
        public IMessage CreateMessage(ProfileMessageSubmitted message)
        {
            throw new NotImplementedException();
        }

        public void CheckSignalRRecipients()
        {
            throw new NotImplementedException();
        }


        public void DispatchMessageToRecipient()
        {
            throw new NotImplementedException();
        }

        public void SaveMessage(IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
