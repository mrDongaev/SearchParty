using Service.Contracts.Hero;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Implementations
{
    public class HeroService : IHeroService
    {
        public Task<HeroDto> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<HeroDto>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
