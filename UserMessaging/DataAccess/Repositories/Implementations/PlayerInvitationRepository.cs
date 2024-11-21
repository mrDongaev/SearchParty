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

        private readonly IMapper _mapper;

        public PlayerInvitationRepository(UserMessagingContext context, IMapper mapper) : base(context)
        {
            _playerInvitations = _context.PlayerInvitations;
            _mapper = mapper;
        }

        public async Task<bool> ClearMessages(ISet<MessageStatus> messageStatues, CancellationToken cancellationToken)
        {
            var messageIds = await _playerInvitations.Where(pi => messageStatues.AsQueryable().Contains(pi.Status)).Select(pi => pi.Id).ToListAsync(cancellationToken);
            return await DeleteRange(messageIds, cancellationToken);
        }

        public async Task<PlayerInvitationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            var message = await Get(id, cancellationToken);
            return message == null ? null : _mapper.Map<PlayerInvitationDto>(message);
        }

        public async Task<ICollection<PlayerInvitationDto>> GetUserMessages(Guid userId, ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            var messages = await _playerInvitations.AsNoTracking()
                .Where(pi => pi.SendingUserId == userId || pi.AcceptingUserId == userId)
                .Where(pi => messageStatuses.AsQueryable().Contains(pi.Status))
                .ToListAsync(cancellationToken);
            return _mapper.Map<ICollection<PlayerInvitationDto>>(messages);
        }

        public async Task<PlayerInvitationDto?> SaveMessage(PlayerInvitationDto messageDto, CancellationToken cancellationToken)
        {
            messageDto.UpdatedAt = DateTime.UtcNow;
            var message = _mapper.Map<PlayerInvitationEntity>(messageDto);
            bool createMessage = true;
            if (messageDto.Id != Guid.Empty)
            {
                createMessage = await Get(messageDto.Id, cancellationToken) == null;
            }
            message = createMessage ? await Add(message, cancellationToken) : await Update(message, cancellationToken);
            return message == null ? null : _mapper.Map<PlayerInvitationDto?>(message);
        }
    }
}
