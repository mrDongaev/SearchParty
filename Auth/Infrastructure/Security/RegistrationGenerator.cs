using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using WebSearchPartyApi;

namespace Infrastructure.Security
{
    public class RegistrationGenerator : IRegistration
    {
        private readonly DataContext _dbContext;

        public RegistrationGenerator(DataContext efDataContext)
        {
            _dbContext = efDataContext;
        }

        public async Task SaveChangesToDbAsync()
        {
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
