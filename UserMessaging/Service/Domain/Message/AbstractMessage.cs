using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Domain.States.Interfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Services.Interfaces.MessageManagement;
using System.Collections.Concurrent;

namespace Service.Domain.Message
{
    public abstract class AbstractMessage<TMessageDto> : IDisposable where TMessageDto : MessageDto
    {
        public Guid Id { get; protected set; }

        public Guid SendingUserId { get; protected set; }

        public Guid AcceptingUserId { get; protected set; }

        public PositionName PositionName { get; protected set; }

        public MessageStatus Status { get; set; }

        public DateTime IssuedAt { get; protected set; }

        public DateTime ExpiresAt { get; protected set; }

        public DateTime UpdatedAt { get; protected set; }

        protected readonly ConcurrentQueue<Func<Task>> _taskQueue = new ConcurrentQueue<Func<Task>>();

        protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        protected AbstractMessageState<TMessageDto> State { get; set; }

        public readonly CancellationToken CancellationToken;

        public readonly IServiceProvider ServiceProvider;

        public readonly IUserHttpContext UserContext;

        public abstract MessageDto MessageDto { get; }

        private bool _disposed = false;

        public AbstractMessage(TMessageDto messageDto, IServiceProvider serviceProvider, IUserHttpContext userContext, CancellationToken cancellationToken)
        {
            Id = messageDto.Id;
            SendingUserId = messageDto.SendingUserId;
            AcceptingUserId = messageDto.AcceptingUserId;
            PositionName = messageDto.PositionName;
            Status = messageDto.Status;
            IssuedAt = messageDto.IssuedAt;
            ExpiresAt = messageDto.ExpiresAt;
            UpdatedAt = messageDto.UpdatedAt;
            ServiceProvider = serviceProvider;
            UserContext = userContext;
            CancellationToken = cancellationToken;
        }

        public void ChangeState(AbstractMessageState<TMessageDto> state)
        {
            State = state;
        }

        protected async Task<T> Execute<T>(Func<Task<T>> task)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(AbstractMessage<TMessageDto>));
            }

            var tcs = new TaskCompletionSource<T>();
            _taskQueue.Enqueue(async () =>
            {
                try
                {
                    T result = await task();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            await _semaphore.WaitAsync(CancellationToken);
            try
            {
                while (_taskQueue.TryDequeue(out var queuedTask))
                {
                    await queuedTask();
                }
            }
            finally
            {
                _semaphore.Release();
            }

            return await tcs.Task;
        }

        public Task<ActionResponse<TMessageDto>> Accept()
        {
            return Execute(State.Accept);
        }

        public Task<ActionResponse<TMessageDto>> Reject()
        {
            return Execute(State.Reject);
        }

        public Task<ActionResponse<TMessageDto>> Rescind()
        {
            return Execute(State.Rescind);
        }

        public abstract Task<TMessageDto?> SaveToDatabase();

        public abstract Task TrySendToUser();

        public void Dispose()
        {
            if (_disposed) return;

            _semaphore?.Dispose();
            _taskQueue.Clear();
            _disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}
