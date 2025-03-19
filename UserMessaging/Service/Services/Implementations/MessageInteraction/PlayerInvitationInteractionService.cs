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
    public class PlayerInvitationInteractionService(IPlayerInvitationRepository playerInvitationRepo, IUserHttpContext userContext, PlayerInvitationManager manager) : IPlayerInvitationInteractionService
    {
        public async Task<Result<PlayerInvitationDto?>> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            var message = await playerInvitationRepo.GetMessage(id, cancellationToken);
            if (message == null)
            {
                return Result.Fail<PlayerInvitationDto?>(new EntityNotFoundError("Player invitation with the given ID has not been found"));
            }
            if (message.AcceptingUserId != userContext.UserId && message.SendingUserId != userContext.UserId)
            {
                return Result.Fail<PlayerInvitationDto?>(new UnauthorizedError());
            }
            return Result.Ok(message);
        }

        public async Task<Result<ICollection<PlayerInvitationDto>>> GetUserMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
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
                return Result.Fail<ICollection<PlayerInvitationDto>>(new UnauthorizedError());
            }

            var messages = await playerInvitationRepo.GetUserMessages(userContext.UserId, messageStatuses, cancellationToken);
            if (messages == null || messages.Count == 0)
            {
                return Result.Fail<ICollection<PlayerInvitationDto>>(new EntitiesNotFoundError("Player invitations of the given user have not been found"));
            }
            return Result.Ok(messages);
        }

        public async Task<Result<PlayerInvitationDto?>> Accept(Guid id, CancellationToken cancellationToken)
        {
            var messageDtoResult = await GetMessage(id, cancellationToken);
            if (messageDtoResult.IsFailed)
            {
                return messageDtoResult;
            }
            if (messageDtoResult.Value?.AcceptingUserId != userContext.UserId)
            {
                return Result.Fail<PlayerInvitationDto?>(new UnauthorizedError());
            }
            var messageDomainObj = await manager.GetOrCreateMessage(id, userContext.GetPersistentData(), cancellationToken);
            if (messageDomainObj != null)
            {
                return await messageDomainObj.Accept();
            }
            else
            {
                return Result.Fail<PlayerInvitationDto?>(new EntityNotFoundError("Player invitation with the given ID has not been found"));
            }
        }

        public async Task<Result<PlayerInvitationDto?>> Reject(Guid id, CancellationToken cancellationToken)
        {
            var messageDtoResult = await GetMessage(id, cancellationToken);
            if (messageDtoResult.IsFailed)
            {
                return messageDtoResult;
            }
            if (messageDtoResult.Value?.AcceptingUserId != userContext.UserId)
            {
                return Result.Fail<PlayerInvitationDto?>(new UnauthorizedError());
            }
            var messageDomainObj = await manager.GetOrCreateMessage(id, userContext.GetPersistentData(), cancellationToken);
            if (messageDomainObj != null)
            {
                return await messageDomainObj.Reject();
            }
            else
            {
                return Result.Fail<PlayerInvitationDto?>(new EntityNotFoundError("Player invitation with the given ID has not been found"));
            }
        }

        public async Task<Result<PlayerInvitationDto?>> Rescind(Guid id, CancellationToken cancellationToken)
        {
            var messageDtoResult = await GetMessage(id, cancellationToken);
            if (messageDtoResult.IsFailed)
            {
                return messageDtoResult;
            }
            if (messageDtoResult.Value?.SendingUserId != userContext.UserId)
            {
                return Result.Fail<PlayerInvitationDto?>(new UnauthorizedError());
            }
            var messageDomainObj = await manager.GetOrCreateMessage(id, userContext.GetPersistentData(), cancellationToken);
            if (messageDomainObj != null)
            {
                return await messageDomainObj.Rescind();
            }
            else
            {
                return Result.Fail<PlayerInvitationDto?>(new EntityNotFoundError("Player invitation with the given ID has not been found"));
            }
        }
    }
}
