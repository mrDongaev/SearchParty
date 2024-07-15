using DataAccess.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class Position : IEntity<int>
    {
        public int Id { get; set; }

        public Enums.Position Name { get; set; }
    }
}
