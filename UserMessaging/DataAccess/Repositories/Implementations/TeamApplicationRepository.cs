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

        public TeamApplicationRepository(UserMessagingContext context, IMapper mapper) : base(context)
        {
            _teamApplications = context.TeamApplications;
        }

        public Task ClearMessages(ISet<MessageStatus> messageTypes, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TeamApplicationDto?> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TeamApplicationDto>> GetUserMessages(Guid userId, MessageStatus messageType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TeamApplicationDto?> SaveMessage(TeamApplicationDto message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
