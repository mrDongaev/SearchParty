using FluentResults;

namespace Library.Services.Interfaces
{
    /// <summary>
    /// Базовый сервис
    /// </summary>
    public interface IService<TGetDto, TId>
    {
        /// <summary>
        /// Получить сущность
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>ДТО сущности</returns>
        Task<Result<TGetDto?>> Get(TId id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все сущности
        /// </summary>
        /// <returns>Список ДТО сущностей</returns>
        Task<Result<ICollection<TGetDto>>> GetAll(CancellationToken cancellationToken);
    }
}
