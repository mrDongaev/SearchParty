using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Models.Enums;
using Library.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implementations
{
    public class PlayerInvitationRepository(UserMessagingContext context) : Repository<UserMessagingContext, PlayerInvitation, Guid>(context), IPlayerInvitationRepository
    {
    }
}
