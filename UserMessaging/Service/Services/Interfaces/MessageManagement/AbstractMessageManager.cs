using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Domain.Message;
using Service.Dtos.Message;
using System.Collections.Concurrent;

namespace Service.Services.Interfaces.MessageManagement
{
    public abstract class AbstractMessageManager<TMessageDto> : IDisposable
        where TMessageDto : MessageDto
    {
        protected readonly ConcurrentDictionary<Guid, AbstractMessage<TMessageDto>> _currentMessages = new();

        protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        protected readonly IServiceScopeFactory _scopeFactory;

        private bool _disposed = false;

        private readonly TimeSpan _idleCheckInterval = TimeSpan.FromSeconds(2);

        private readonly TimeSpan _clearInterval = TimeSpan.FromMinutes(60);

        public AbstractMessageManager(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            StartBackgroundTasks();
        }

        private void StartBackgroundTasks()
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (_disposed) break;
                    await Task.Delay(_idleCheckInterval);
                    try
                    {
                        await _semaphore.WaitAsync();
                        CheckAndReleaseIdleMessages();
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            });

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (_disposed) break;
                    await Task.Delay(_clearInterval);
                    try
                    {
                        await _semaphore.WaitAsync();
                        await ClearResolvedMessages();
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            });
        }

        private void CheckAndReleaseIdleMessages()
        {
            foreach (var kvp in _currentMessages)
            {
                var messageId = kvp.Key;
                var message = kvp.Value;

                if (message.IsIdle())
                {
                    message.Dispose();
                    _currentMessages.TryRemove(messageId, out _);
                }
            }
        }

        protected abstract Task ClearResolvedMessages();

        public async Task<AbstractMessage<TMessageDto>?> GetOrCreateMessage(Guid id, IUserHttpContext userContext, CancellationToken cancellationToken)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(AbstractMessageManager<TMessageDto>));
            }

            AbstractMessage<TMessageDto>? message;

            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                if (_currentMessages.TryGetValue(id, out message))
                {
                    return message;
                }
                else
                {
                    message = await CreateMessageModel(id, userContext, cancellationToken);
                    if (message != null) _currentMessages.TryAdd(id, message);
                }
                return message;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        protected abstract Task<AbstractMessage<TMessageDto>?> CreateMessageModel(Guid id, IUserHttpContext userContext, CancellationToken cancellationToken);

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
