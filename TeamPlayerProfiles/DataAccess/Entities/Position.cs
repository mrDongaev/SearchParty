using Common.Enums;
using Library.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class Position : IEntity<int>
    {
        public int Id { get; set; }

        public PositionName Name { get; set; }
    }
}
