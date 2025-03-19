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
                "UnauthorizedError" => new UnauthorizedError(message),
                "EntityNotFoundError" => new EntityNotFoundError(message),
                "EntitiesNotFoundError" => new EntitiesNotFoundError(message),
                "EntitiesForQueryNotFoundError" => new EntitiesForQueryNotFoundError(message),
                "EntityRangeNotFoundError" => new EntityRangeNotFoundError(message),
                "EntityAlreadyExistsError" => new EntityAlreadyExistsError(message),
                "ValidationError" => new ValidationError(message),
                "NoPendingMessageError" => new NoPendingMessageError(message),
                "PendingMessageExistsError" => new PendingMessageExistsError(message),
                "SelfMessagingError" => new SelfMessagingError(message),
                "TeamContainsPlayerError" => new TeamContainsPlayerError(message),
                "TeamCountOverflowError" => new TeamCountOverflowError(message),
                "TeamOwnerNotPresentError" => new TeamOwnerNotPresentError(message),
                "TeamPositionOverlapError" => new TeamPositionOverlapError(message),
                "HttpRequestFailedError" => new HttpRequestFailedError(message),
                "MessageAcceptedError" => new MessageAcceptedError(message),
                "MessageAcceptFailedError" => new MessageAcceptFailedError(message),
                "MessageExpiredError" => new MessageExpiredError(message),
                "MessageRejectedError" => new MessageRejectedError(message),
                "MessageRejectFailedError" => new MessageRejectFailedError(message),
                "MessageRescindedError" => new MessageRescindedError(message),
                "MessageRescindFailedError" => new MessageRescindFailedError(message),
                "Error" => new Error(message),
                _ => new Error(message),
            };
        }

        public static Success GetSuccessByKey(string key, string message)
        {
            return key switch
            {
                "MessageSentSuccess" => new MessageSentSuccess(message),
                "MessageAcceptedSuccess" => new MessageAcceptedSuccess(message),
                "MessageRejectedSuccess" => new MessageRejectedSuccess(message),
                "MessageRescindedSuccess" => new MessageRescindedSuccess(message),
                "Success" => new Success(message),
                _ => new Success(message),
            };
        }
    }
}
