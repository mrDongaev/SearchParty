using FluentResults;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Results.Errors.Http;
using Library.Results.Errors.Messages;
using Library.Results.Errors.Validation;
using Library.Results.Errors.Validation.Message;
using Library.Results.Errors.Validation.Team;
using Library.Results.Successes.Messages;

namespace Library.Results.Utils
{
    public static class ResultUtils
    {
        public static Error GetErrorByKey(string key, string message)
        {
            return key switch
            {
                nameof(UnauthorizedError) => new UnauthorizedError(message),
                nameof(EntityNotFoundError) => new EntityNotFoundError(message),
                nameof(EntitiesNotFoundError) => new EntitiesNotFoundError(message),
                nameof(EntitiesForQueryNotFoundError) => new EntitiesForQueryNotFoundError(message),
                nameof(EntityRangeNotFoundError) => new EntityRangeNotFoundError(message),
                nameof(EntityAlreadyExistsError) => new EntityAlreadyExistsError(message),
                nameof(ValidationError) => new ValidationError(message),
                nameof(NoPendingMessageError) => new NoPendingMessageError(message),
                nameof(PendingMessageExistsError) => new PendingMessageExistsError(message),
                nameof(SelfMessagingError) => new SelfMessagingError(message),
                nameof(TeamContainsPlayerError) => new TeamContainsPlayerError(message),
                nameof(TeamCountOverflowError) => new TeamCountOverflowError(message),
                nameof(TeamOwnerNotPresentError) => new TeamOwnerNotPresentError(message),
                nameof(TeamPositionOverlapError) => new TeamPositionOverlapError(message),
                nameof(HttpRequestFailedError) => new HttpRequestFailedError(message),
                nameof(MessageAcceptFailedError) => new MessageAcceptFailedError(message),
                nameof(MessageRejectFailedError) => new MessageRejectFailedError(message),
                nameof(MessageRescindFailedError) => new MessageRescindFailedError(message),
                nameof(Error) => new Error(message),
                _ => new Error(message),
            };
        }

        public static Success GetSuccessByKey(string key, string message)
        {
            return key switch
            {
                nameof(MessageSentSuccess) => new MessageSentSuccess(message),
                nameof(MessageAcceptedSuccess) => new MessageAcceptedSuccess(message),
                nameof(MessageRejectedSuccess) => new MessageRejectedSuccess(message),
                nameof(MessageRescindedSuccess) => new MessageRescindedSuccess(message),
                nameof(MessageAlreadyAcceptedSuccess) => new MessageAlreadyAcceptedSuccess(message),
                nameof(MessageAlreadyExpiredSuccess) => new MessageAlreadyExpiredSuccess(message),
                nameof(MessageAlreadyRejectedSuccess) => new MessageAlreadyRejectedSuccess(message),
                nameof(MessageAlreadyRescindedSuccess) => new MessageAlreadyRescindedSuccess(message),
                nameof(Success) => new Success(message),
                _ => new Success(message),
            };
        }
    }
}
