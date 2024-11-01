using Library.Entities.Interfaces;
using Library.Models.Enums;

namespace DataAccess.Entities
{
    public class Position : IEntity<int>
    {
        public int Id { get; set; }

        public PositionName Name { get; set; }
    }
}
