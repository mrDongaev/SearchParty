using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Repositories.Implementations;

namespace DataAccess.Repositories.Implementations
{
    public class PlayerInvitationRepository(UserMessagingContext context) : Repository<UserMessagingContext, PlayerInvitation, Guid>(context), IPlayerInvitationRepository
    {
    }
}
