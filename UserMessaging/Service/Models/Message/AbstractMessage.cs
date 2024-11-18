using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.States.Interfaces;
using System.Collections.Concurrent;

namespace Service.Models.Message
{
    public abstract class AbstractMessage
    {
        public Guid Id { get; set; }

        public Guid SendingUserId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public PositionName PositionName { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        protected readonly ConcurrentQueue<Func<Task>> _taskQueue = new ConcurrentQueue<Func<Task>>();

        protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        protected AbstractMessageState State { get; set; }

        public readonly CancellationToken CancellationToken;

        public readonly IServiceProvider ServiceProvider;

        public readonly IUserHttpContext UserContext;

        public abstract MessageDto MessageDto { get; }

        public AbstractMessage(IServiceProvider serviceProvider, IUserHttpContext userContext, AbstractMessageState startingState, CancellationToken cancellationToken)
        {
            ServiceProvider = serviceProvider;
            UserContext = userContext;
            CancellationToken = cancellationToken;
            State = startingState;
        }

        public void ChangeState(AbstractMessageState state)
        {
            State = state;
        }

        protected async Task<T> Execute<T>(Func<Task<T>> task)
        {
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

            await _semaphore.WaitAsync();
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

        public Task<ActionResponse<MessageDto>> Accept()
        {
            return Execute(State.Accept);
        }

        public Task<ActionResponse<MessageDto>> Reject()
        {
            return Execute(State.Reject);
        }

        public Task<ActionResponse<MessageDto>> Rescind()
        {
            return Execute(State.Rescind);
        }

        public abstract Task TrySendToUser();

        public abstract Task SaveToDatabase();
    }
}
