using FluentResults;
using Library.Exceptions;
using Library.Models.Enums;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;
using Service.Services.Implementations.MessageManagement;
using Service.Services.Interfaces.MessageInteraction;

namespace Service.Services.Implementations.MessageInteraction
{
    public class TeamApplicationInteractionService(ITeamApplicationRepository teamApplicationRepo, IUserHttpContext userContext, TeamApplicationManager manager) : ITeamApplicationInteractionService
    {
        public async Task<Result<TeamApplicationDto?>> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            var message = await teamApplicationRepo.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return Result.Fail<TeamApplicationDto?>(new EntityNotFoundError("Team application with the given ID has not been found")).WithValue(null);
            }
            if (message.AcceptingUserId != userContext.UserId && message.SendingUserId != userContext.UserId)
            {
                return Result.Fail<TeamApplicationDto?>(new UnauthorizedError()).WithValue(null);
            }
            return Result.Ok(message);
        }

        public async Task<Result<ICollection<TeamApplicationDto>>> GetUserMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            foreach (var status in messageStatuses)
            {
                if (!Enum.IsDefined(status))
                {
                    throw new InvalidEnumMemberException(status.ToString(), typeof(PositionName).Name);
                }
            }

            if (Guid.Empty == userContext.UserId)
            {
                return Result.Fail<ICollection<TeamApplicationDto>>(new UnauthorizedError()).WithValue([]);
            }

            var messages = await teamApplicationRepo.GetUserMessages(userContext.UserId, messageStatuses, cancellationToken);
            if (messages == null || messages.Count == 0)
            {
                return Result.Fail<ICollection<TeamApplicationDto>>(new EntitiesNotFoundError("Team applications of the given user have not been found")).WithValue([]);
            }
            return Result.Ok(messages);
        }

        public async Task<Result<TeamApplicationDto?>> Accept(Guid id, CancellationToken cancellationToken)
        {
            var messageDtoResult = await GetMessage(id, cancellationToken);
            if (messageDtoResult.IsFailed)
            {
                return messageDtoResult;
            }
            if (messageDtoResult.Value?.AcceptingUserId != userContext.UserId)
            {
                return Result.Fail<TeamApplicationDto?>(new UnauthorizedError()).WithValue(null);
            }
            var messageDomainObj = await manager.GetOrCreateMessage(id, userContext.GetPersistentData(), cancellationToken);
            if (messageDomainObj != null)
            {
                return await messageDomainObj.Accept();
            }
            else
            {
                return Result.Fail<TeamApplicationDto?>(new EntityNotFoundError("Team application with the given ID has not been found")).WithValue(null);
            }
        }

        public async Task<Result<TeamApplicationDto?>> Reject(Guid id, CancellationToken cancellationToken)
        {
            var messageDtoResult = await GetMessage(id, cancellationToken);
            if (messageDtoResult.IsFailed)
            {
                return messageDtoResult;
            }
            if (messageDtoResult.Value?.AcceptingUserId != userContext.UserId)
            {
                return Result.Fail<TeamApplicationDto?>(new UnauthorizedError()).WithValue(null);
            }
            var messageDomainObj = await manager.GetOrCreateMessage(id, userContext.GetPersistentData(), cancellationToken);
            if (messageDomainObj != null)
            {
                return await messageDomainObj.Reject();
            }
            else
            {
                return Result.Fail<TeamApplicationDto?>(new EntityNotFoundError("Team application with the given ID has not been found")).WithValue(null);
            }
        }

        public async Task<Result<TeamApplicationDto?>> Rescind(Guid id, CancellationToken cancellationToken)
        {
            var messageDtoResult = await GetMessage(id, cancellationToken);
            if (messageDtoResult.IsFailed)
            {
                return messageDtoResult;
            }
            if (messageDtoResult.Value?.SendingUserId != userContext.UserId)
            {
                return Result.Fail<TeamApplicationDto?>(new UnauthorizedError()).WithValue(null);
            }
            var messageDomainObj = await manager.GetOrCreateMessage(id, userContext.GetPersistentData(), cancellationToken);
            if (messageDomainObj != null)
            {
                return await messageDomainObj.Rescind();
            }
            else
            {
                return Result.Fail<TeamApplicationDto?>(new EntityNotFoundError("Team application with the given ID has not been found")).WithValue(null);
            }
        }
    }
}
