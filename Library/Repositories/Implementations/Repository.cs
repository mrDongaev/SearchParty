using Library.Entities.Interfaces;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories.Implementations
{
    public abstract class Repository<TContext, TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TContext : DbContext
    {
        protected TContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns> Cущность. </returns>
        public virtual async Task<TEntity?> Get(TId id, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
        }

        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Список сущностей. </returns>
        public virtual async Task<ICollection<TEntity>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Добавить в базу одну сущность.
        /// </summary>
        /// <param name="entity"> Сущность для добавления. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Добавленная сущность. </returns>
        public virtual async Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken)
        {
            var addedModel = (await _dbSet.AddAsync(entity, cancellationToken));
            await _context.SaveChangesAsync(cancellationToken);
            addedModel.State = EntityState.Detached;
            return addedModel.Entity;
        }

        /// <summary>
        /// Добавить в базу массив сущностей.
        /// </summary>
        /// <param name="entities"> Массив сущностей. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        public virtual async Task<bool> AddRange(ICollection<TEntity> entities, CancellationToken cancellationToken)
        {
            if (entities == null || entities.Count == 0)
            {
                return false;
            }
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Для сущности проставить состояние - что она изменена.
        /// </summary>
        /// <param name="entity"> Сущность для изменения. </param>
        public virtual async Task<TEntity?> Update(TEntity entity, CancellationToken cancellationToken)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            entry.State = EntityState.Detached;
            return entry.Entity;
        }

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="id"> Id удалённой сущности. </param>
        /// <returns> Была ли сущность удалена. </returns>
        public virtual async Task<bool> Delete(TId id, CancellationToken cancellationToken)
        {
            var obj = await _dbSet.FindAsync(id, cancellationToken);
            if (obj == null)
            {
                return false;
            }
            _dbSet.Remove(obj);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Удалить сущности.
        /// </summary>
        /// <param name="entities"> Коллекция сущностей для удаления. </param>
        /// <returns> Была ли операция завершена успешно. </returns>
        public virtual async Task<bool> DeleteRange(ICollection<TEntity> entities, CancellationToken cancellationToken)
        {
            if (entities == null || entities.Count == 0)
            {
                return false;
            }
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
