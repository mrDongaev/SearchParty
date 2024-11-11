using Library.Entities.Interfaces;

namespace DataAccess.Entities.Interfaces
{
    public interface IProfile : IEntity<Guid>, IUpdateable
    {
        Guid UserId { get; set; }

        User User { get; set; }

        string? Name { get; set; }

        string? Description { get; set; }

        bool? Displayed { get; set; }

    }
}
