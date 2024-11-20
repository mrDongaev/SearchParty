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
    public class TeamApplicationRepository : Repository<UserMessagingContext, TeamApplicationEntity, Guid>, ITeamApplicationRepository
    {
        private readonly DbSet<TeamApplicationEntity> _teamApplications;

        private readonly IMapper _mapper;

        public TeamApplicationRepository(UserMessagingContext context, IMapper mapper) : base(context)
        {
            _teamApplications = _context.TeamApplications;
            _mapper = mapper;
        }
        public async Task<bool> ClearMessages(ISet<MessageStatus> messageStatues, CancellationToken cancellationToken)
        {
            var messageIds = await _teamApplications.Where(pi => messageStatues.Contains(pi.Status)).Select(pi => pi.Id).ToListAsync(cancellationToken);
            return await DeleteRange(messageIds, cancellationToken);
        }

        public async Task<TeamApplicationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            var messages = await Get(id, cancellationToken);
            return _mapper.Map<TeamApplicationDto>(messages);
        }

        public async Task<ICollection<TeamApplicationDto>> GetUserMessages(Guid userId, ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            var messages = await _teamApplications.AsNoTracking()
                .Where(pi => pi.SendingUserId == userId || pi.AcceptingUserId == userId)
                .Where(pi => messageStatuses.Contains(pi.Status))
                .ToListAsync(cancellationToken);
            return _mapper.Map<ICollection<TeamApplicationDto>>(messages);
        }

        public async Task<TeamApplicationDto?> SaveMessage(TeamApplicationDto messageDto, CancellationToken cancellationToken)
        {
            messageDto.UpdatedAt = DateTime.UtcNow;
            var message = _mapper.Map<TeamApplicationEntity>(messageDto);
            bool createMessage = true;
            if (messageDto.Id != Guid.Empty)
            {
                createMessage = await Get(messageDto.Id, cancellationToken) == null;
            }
            message = createMessage ? await Add(message, cancellationToken) : await Update(message, cancellationToken);
            return message == null ? null : _mapper.Map<TeamApplicationDto?>(message);
        }
    }
}
