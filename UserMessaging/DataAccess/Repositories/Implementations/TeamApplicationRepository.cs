using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Entities.Interfaces;
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
    public class TeamApplicationRepository(UserMessagingContext context) : Repository<UserMessagingContext, TeamApplication, Guid>(context), ITeamApplicationRepository
    {
    }
}
