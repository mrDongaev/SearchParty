namespace Service.Services.Interfaces.Common
{
    public interface IBoardService<TGetDto>
    {
        /// <summary>
        /// Выставить профиль на показ
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="displayed">Выставить/убрать</param>
        /// <returns></returns>
        Task<TGetDto> SetDisplay(Guid id, bool displayed, CancellationToken cancellationToken);
    }
}
