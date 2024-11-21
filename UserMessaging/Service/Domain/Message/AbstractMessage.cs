﻿using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Domain.States.Interfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Services.Interfaces.MessageManagement;
using System.Collections.Concurrent;
using System.Security.Cryptography;

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

        private int _activeThreads = 0;

        private bool _isProcessingQueue = false;

        private bool _processingStarted = false;

        protected AbstractMessageState<TMessageDto> State { get; set; }

        public readonly CancellationToken CancellationToken;

        public readonly IServiceScopeFactory ScopeFactory;

        public readonly IUserHttpContext UserContext;

        public abstract MessageDto MessageDto { get; }

        private bool _disposed = false;

        public AbstractMessage(TMessageDto messageDto, IServiceScopeFactory scopeFactory, IUserHttpContext userContext, CancellationToken cancellationToken)
        {
            Id = messageDto.Id;
            SendingUserId = messageDto.SendingUserId;
            AcceptingUserId = messageDto.AcceptingUserId;
            PositionName = messageDto.PositionName;
            Status = messageDto.Status;
            IssuedAt = messageDto.IssuedAt;
            ExpiresAt = messageDto.ExpiresAt;
            UpdatedAt = messageDto.UpdatedAt;
            ScopeFactory = scopeFactory;
            UserContext = userContext;
            CancellationToken = cancellationToken;
            State = CreateNewMessageState(Status);
        }

        public void ChangeState(MessageStatus status)
        {
            Status = status;
            State = CreateNewMessageState(status);
        }

        protected async Task<TResult> Execute<TResult>(Func<Task<TResult>> operation)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(AbstractMessage<TMessageDto>));
            }

            var tcs = new TaskCompletionSource<TResult>();

            Interlocked.Increment(ref _activeThreads);

            _taskQueue.Enqueue(async () =>
            {
                try
                {
                    var result = await operation();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
                finally
                {
                    Interlocked.Decrement(ref _activeThreads);
                    await ProcessQueue();
                }
            });

            StartProcessingQueueIfNotRunning();

            return await tcs.Task;
        }

        private void StartProcessingQueueIfNotRunning()
        {
            lock (_taskQueue)
            {
                if (!_isProcessingQueue)
                {
                    _isProcessingQueue = true;
                    _processingStarted = true;
                    _ = ProcessQueue();
                }
            }
        }

        private async Task ProcessQueue()
        {
            while (true)
            {
                if (!_taskQueue.TryDequeue(out var task))
                {
                    lock (_taskQueue)
                    {
                        _isProcessingQueue = false;
                    }
                    return;
                }

                await _semaphore.WaitAsync();
                try
                {
                    await task();
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        public bool IsIdle()
        {
            return _processingStarted && !_isProcessingQueue && _taskQueue.IsEmpty && _activeThreads == 0;
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

        protected abstract AbstractMessageState<TMessageDto> CreateNewMessageState(MessageStatus status);

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
