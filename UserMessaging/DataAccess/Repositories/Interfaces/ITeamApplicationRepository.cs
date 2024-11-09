using DataAccess.Entities;
using DataAccess.Entities.Interfaces;
using Library.Entities.Interfaces;
using Library.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITeamApplicationRepository : IRepository<TeamApplication, Guid>
    {
    }
}
