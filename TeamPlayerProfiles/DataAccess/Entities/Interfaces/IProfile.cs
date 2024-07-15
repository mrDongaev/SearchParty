using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities.Interfaces
{
    public interface IProfile : IEntity<Guid>, IUpdateable
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }

    }
}
