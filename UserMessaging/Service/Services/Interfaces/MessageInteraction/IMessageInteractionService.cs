﻿using Service.Dtos.ActionResponse;
using Service.Dtos.Message;

namespace Service.Services.Interfaces.MessageInteraction
{
    public interface IMessageInteractionService<TMessageDto> where TMessageDto : PlayerInvitationDto
    {
        Task<TMessageDto?> GetMessage(Guid id, CancellationToken cancellationToken);

        Task<ICollection<TMessageDto>> GetPendingUserMessages(Guid userId, CancellationToken cancellationToken);

        Task<ActionResponse<TMessageDto>> Accept(Guid id, CancellationToken cancellationToken);

        Task<ActionResponse<TMessageDto>> Reject(Guid id, CancellationToken cancellationToken);

        Task<ActionResponse<TMessageDto>> Rescind(Guid id, CancellationToken cancellationToken);
    }
}