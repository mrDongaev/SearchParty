namespace Service.Services.Interfaces
{
    /// <summary>
    /// Базовый интерфейс сервиса профилей
    /// </summary>
    /// <typeparam name="TGetDto">ДТО чтения профиля</typeparam>
    /// <typeparam name="TUpdateDto">ДТО обновления профиля</typeparam>
    /// <typeparam name="TCreateDto">ДТО создания профиля</typeparam>
    public interface IProfileService<TGetDto, TUpdateDto, TCreateDto> : IService<TGetDto, Guid>
    {
        /// <summary>
        /// Получить сущность по ID
        /// </summary>
        /// <param name="id"> Идентификатор</param>
        /// <returns> ДТО удалённого профиля</returns>
        Task<TGetDto> Delete(Guid id);

        /// <summary>
        /// Создать профиль
        /// </summary>
        /// <param name="dto">ДТО создаваемого профиля</param>
        /// <returns>ДТО созданного профиля</returns>
        Task<TGetDto> Create(TCreateDto dto);

        /// <summary>
        /// Обновить данные профиля
        /// </summary>
        /// <param name="dto">ДТО обновляемого профиля</param>
        /// <returns>ДТО обновлённого профиля</returns>
        Task<TGetDto> Update(TUpdateDto dto);
    }
}
