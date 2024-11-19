using Library.Models.API.UserMessaging;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Domain.Message;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace Service.Services.Interfaces.MessageManagement
{
    public abstract class AbstractMessageManager<TMessageDto> : IDisposable
        where TMessageDto : MessageDto
    {
        protected readonly ConcurrentDictionary<Guid, AbstractMessage<TMessageDto>> _currentMessages = new();

        protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private bool _disposed = false;

        public async Task<AbstractMessage<TMessageDto>?> GetOrCreateMessage(Guid id, IServiceProvider serviceProvider, IUserHttpContext userContext, CancellationToken cancellationToken)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(AbstractMessageManager<TMessageDto>));
            }

            AbstractMessage<TMessageDto>? message;

            if (_currentMessages.TryGetValue(id, out message))
            {
                return message;
            }

            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                if (_currentMessages.TryGetValue(id, out message))
                {
                    return message;
                }
                else
                {
                    message = await CreateMessageModel(id, serviceProvider, userContext, cancellationToken);
                    if (message != null) _currentMessages.TryAdd(id, message);
                }
                return message;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        protected abstract Task<AbstractMessage<TMessageDto>?> CreateMessageModel(Guid id, IServiceProvider serviceProvider, IUserHttpContext userContext, CancellationToken cancellationToken);

        public void Dispose()
        {
            if (_disposed) return;

            _semaphore?.Dispose();
            _currentMessages.Clear();
            _disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}
