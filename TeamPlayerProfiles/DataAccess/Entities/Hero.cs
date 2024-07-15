using System.ComponentModel.DataAnnotations;
using DataAccess.Entities.Enums;
using DataAccess.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class Hero : IEntity<int>
    {
        public int Id {  get; set; }

        public string Name { get; set; }

        public MainStat MainStat { get; set; }

        public ICollection<Player> Players { get; set; } = [];

    }
}
