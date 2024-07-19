using DataAccess.Entities.Interfaces;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public abstract class Repository<T, TId>(DbContext context) : IRepository<T, TId> where T
        : class, IEntity<TId>
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns> Cущность. </returns>
        public virtual async Task<T> Get(TId id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Список сущностей. </returns>
        public virtual async Task<ICollection<T>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Добавить в базу одну сущность.
        /// </summary>
        /// <param name="entity"> Сущность для добавления. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Добавленная сущность. </returns>
        public virtual async Task<T> Add(T entity, CancellationToken cancellationToken)
        {
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        /// <summary>
        /// Добавить в базу массив сущностей.
        /// </summary>
        /// <param name="entities"> Массив сущностей. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        public virtual async Task<bool> AddRange(ICollection<T> entities, CancellationToken cancellationToken)
        {
            if (entities == null || !entities.Any())
            {
                return false;
            }
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            return true;
        }

        /// <summary>
        /// Для сущности проставить состояние - что она изменена.
        /// </summary>
        /// <param name="entity"> Сущность для изменения. </param>
        public virtual async Task<T> Update(T entity, CancellationToken cancellationToken)
        {
            var entry = context.Entry(entity);
            entry.State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);
            return entry.Entity;
        }

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="id"> Id удалённой сущности. </param>
        /// <returns> Была ли сущность удалена. </returns>
        public virtual async Task<bool> Delete(TId id, CancellationToken cancellationToken)
        {
            var obj = _dbSet.Find(id);
            if (obj == null)
            {
                return false;
            }
            _dbSet.Remove(obj);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Удалить сущности.
        /// </summary>
        /// <param name="entities"> Коллекция сущностей для удаления. </param>
        /// <returns> Была ли операция завершена успешно. </returns>
        public virtual async Task<bool> DeleteRange(ICollection<T> entities, CancellationToken cancellationToken)
        {
            if (entities == null || !entities.Any())
            {
                return false;
            }
            _dbSet.RemoveRange(entities);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
