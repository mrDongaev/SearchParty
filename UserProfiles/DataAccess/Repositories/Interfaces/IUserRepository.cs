using DataAccess.Entities;
using Library.Entities.Interfaces;
using Library.Repositories.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User, Guid>, IRangeGettable<User, Guid>
    {
        /// <summary>
        /// Получить профили с фильтрацией по MMR
        /// </summary>
        /// <param name="cancellationToken"> Токен отмены. </param>
        /// <returns> Список сущностей. </returns>
        Task<ICollection<T>> GetAll(CancellationToken cancellationToken);
    }
}
