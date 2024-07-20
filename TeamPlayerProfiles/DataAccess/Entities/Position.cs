using DataAccess.Entities.Interfaces;
using EnumPosition = Common.Models.Enums.Position;

namespace DataAccess.Entities
{
    public class Position : IEntity<int>
    {
        public int Id { get; set; }

        public EnumPosition Name { get; set; }
    }
}
