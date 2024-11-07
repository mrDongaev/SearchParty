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
        Task<bool> Delete(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создать профиль
        /// </summary>
        /// <param name="dto">ДТО создаваемого профиля</param>
        /// <returns>ДТО созданного профиля</returns>
        Task<TGetDto> Create(TCreateDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Обновить данные профиля
        /// </summary>
        /// <param name="dto">ДТО обновляемого профиля</param>
        /// <returns>ДТО обновлённого профиля</returns>
        Task<TGetDto?> Update(TUpdateDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Получить ряд сущностей по идентификатору
        /// </summary>
        /// <param name="ids">Список идентификаторов</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список ДТО сущностей</returns>
        Task<ICollection<TGetDto>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken);
    }
}
