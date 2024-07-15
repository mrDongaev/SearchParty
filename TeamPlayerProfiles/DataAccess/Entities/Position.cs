using DataAccess.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class Position : IEntity<int>
    {
        public int Id { get; set; }

        public Enums.Position Name { get; set; }

        public ICollection<Player> Players { get; set; }

        public ICollection<TeamPlayer> TeamPlayers { get; set; }
    }
}
