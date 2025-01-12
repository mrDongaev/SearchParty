using FluentResults;
using Library.Services.Interfaces;

namespace Service.Services.Interfaces.Common
{
    /// <summary>
    /// Базовый интерфейс сервиса профилей
    /// </summary>
    /// <typeparam name="TGetDto">ДТО чтения профиля</typeparam>
    /// <typeparam name="TUpdateDto">ДТО обновления профиля</typeparam>
    /// <typeparam name="TCreateDto">ДТО создания профиля</typeparam>
    public interface IProfileService<TGetDto, TUpdateDto, TCreateDto> : IService<TGetDto, Guid>, IRangeGettable<TGetDto, Guid>
    {
        /// <summary>
        /// Получить сущность по ID
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns> ДТО удалённого профиля</returns>
        Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создать профиль
        /// </summary>
        /// <param name="dto">ДТО создаваемого профиля</param>
        /// <returns>ДТО созданного профиля</returns>
        Task<Result<TGetDto?>> Create(TCreateDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Обновить данные профиля
        /// </summary>
        /// <param name="dto">ДТО обновляемого профиля</param>
        /// <returns>ДТО обновлённого профиля</returns>
        Task<Result<TGetDto?>> Update(TUpdateDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все профили пользователя по ID пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Список ДТО сущностей</returns>
        Task<Result<ICollection<TGetDto>>> GetProfilesByUserId(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получить ID пользователя создателя сущности по ID сущности
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns> GUID пользователя</returns>
        Task<Result<Guid?>> GetProfileUserId(Guid profileId, CancellationToken cancellationToken);
    }
}
