﻿using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;

namespace Service.Domain.Message
{
    public class TeamApplication : AbstractMessage<TeamApplicationDto>
    {
        public TeamApplication(TeamApplicationDto messageDto, IServiceProvider serviceProvider, IUserHttpContext userContext, CancellationToken cancellationToken)
            : base(messageDto, serviceProvider, userContext, cancellationToken)
        {
            ApplyingPlayerId = messageDto.ApplyingPlayerId;
            AcceptingTeamId = messageDto.AcceptingTeamId;
        }

        public Guid ApplyingPlayerId { get; protected set; }

        public Guid AcceptingTeamId { get; protected set; }

        public override TeamApplicationDto MessageDto => new TeamApplicationDto
        {
            Id = Id,
            AcceptingUserId = AcceptingUserId,
            SendingUserId = SendingUserId,
            AcceptingTeamId = AcceptingTeamId,
            ApplyingPlayerId = ApplyingPlayerId,
            PositionName = PositionName,
            Status = Status,
            IssuedAt = IssuedAt,
            ExpiresAt = ExpiresAt,
            UpdatedAt = UpdatedAt,
        };

        public async override Task<TeamApplicationDto?> SaveToDatabase()
        {
            using var scope = ServiceProvider.CreateScope();
            var teamApplicationService = scope.ServiceProvider.GetRequiredService<ITeamApplicationRepository>();
            var messageDto = await teamApplicationService.SaveMessage(MessageDto, CancellationToken);
            if (messageDto != null)
            {
                Status = messageDto.Status;
                UpdatedAt = messageDto.UpdatedAt;
            }
            return messageDto;
        }

        public async override Task TrySendToUser()
        {
            await Task.CompletedTask;
            //todo
        }
    }
}
