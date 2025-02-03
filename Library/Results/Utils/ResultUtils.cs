using FluentResults;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Results.Errors.Http;
using Library.Results.Errors.Validation;
using Library.Results.Errors.Validation.Message;
using Library.Results.Errors.Validation.Team;
using Library.Results.Successes.Message;

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
                "NoPendingInvitationError" => new NoPendingInvitationError(message),
                "NoPendingApplicationError" => new NoPendingApplicationError(message),
                "PendingMessageExistsError" => new PendingMessageExistsError(message),
                "PendingInvitationExistsError" => new PendingInvitationExistsError(message),
                "PendingApplicationExistsError" => new PendingApplicationExistsError(message),
                "SelfMessagingError" => new SelfMessagingError(message),
                "SelfInvitationError" => new SelfInvitationError(message),
                "SelfApplicationError" => new SelfApplicationError(message),
                "TeamContainsPlayerError" => new TeamContainsPlayerError(message),
                "TeamCountOverflowError" => new TeamCountOverflowError(message),
                "TeamOwnerNotPresentError" => new TeamOwnerNotPresentError(message),
                "TeamPositionOverlapError" => new TeamPositionOverlapError(message),
                "HttpRequestFailedError" => new HttpRequestFailedError(message),
                "Error" => new Error(message),
                _ => new Error(message),
            };
        }

        public static Success GetSuccessByKey(string key, string message)
        {
            return key switch
            {
                "MessageSentSuccess" => new MessageSentSuccess(message),
                "InvitationSentSuccess" => new InvitationSentSuccess(message),
                "ApplicationSentSuccess" => new ApplicationSentSuccess(message),
                "Success" => new Success(message),
                _ => new Success(message),
            };
        }
    }
}
