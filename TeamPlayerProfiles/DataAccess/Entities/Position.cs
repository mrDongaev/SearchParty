using Common.Models.Enums;
using DataAccess.Entities.Interfaces;

namespace DataAccess.Entities
{
    public class Position : IEntity<int>
    {
        public int Id { get; set; }

        public PositionName Name { get; set; }
    }
}
