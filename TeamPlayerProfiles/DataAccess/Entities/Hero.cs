using Common.Models.Enums;
using DataAccess.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class Hero : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MainStat MainStat { get; set; }
    }
}
