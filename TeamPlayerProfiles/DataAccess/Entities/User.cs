using Library.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public uint Mmr {  get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<Player> Players { get; protected set; } = [];

        public ICollection<Team> Teams { get; protected set; } = [];
    }
}
