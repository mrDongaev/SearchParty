using Library.Models.API.UserMessaging;
using MassTransit;
using Microsoft.Extensions.Logging;
using Service.Models;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Implementations
{
    public class ProfileMessageConsumer(IMessageProcessor messageProcessor, ILogger logger) : IConsumer<ProfileMessageSubmitted>
    {
        public async Task Consume(ConsumeContext<ProfileMessageSubmitted> context)
        {
            var receivedMessage = context.Message;
            logger.LogInformation($"Received ")
            IMessage message = messageProcessor.CreateMessage(receivedMessage);
        }
    }
}
