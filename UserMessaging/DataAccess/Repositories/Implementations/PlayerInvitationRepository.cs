using AutoMapper;
using DataAccess.Context;
using DataAccess.Entities;
using Library.Models.Enums;
using Library.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;

namespace DataAccess.Repositories.Implementations
{
    public class PlayerInvitationRepository : Repository<UserMessagingContext, PlayerInvitationEntity, Guid>, IPlayerInvitationRepository
    {
        private readonly DbSet<PlayerInvitationEntity> _playerInvitations;

        public PlayerInvitationRepository(UserMessagingContext context, IMapper mapper) : base(context)
        {
            _playerInvitations = context.PlayerInvitations;
        }

        public Task ClearMessages(ISet<MessageStatus> messageTypes, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PlayerInvitationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PlayerInvitationDto>> GetUserMessages(Guid userId, MessageStatus messageType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<PlayerInvitationDto?> IMessageRepository<PlayerInvitationDto>.SaveMessage(PlayerInvitationDto message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
