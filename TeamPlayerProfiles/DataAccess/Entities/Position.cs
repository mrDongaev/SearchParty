using DataAccess.Entities.Interfaces;
using Common.Models.Enums;

namespace DataAccess.Entities
{
    public class Position : IEntity<int>
    {
        public int Id { get; set; }

        public PositionName Name { get; set; }
    }
}
