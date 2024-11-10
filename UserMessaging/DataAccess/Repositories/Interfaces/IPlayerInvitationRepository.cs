﻿using DataAccess.Entities;
using Library.Repositories.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlayerInvitationRepository : IRepository<PlayerInvitation, Guid>
    {
    }
}
