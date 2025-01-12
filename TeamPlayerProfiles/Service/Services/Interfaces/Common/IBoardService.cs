using Common.Models;
using FluentResults;
using Library.Models;

namespace Service.Services.Interfaces.Common
{
    public interface IBoardService<TGetDto, TProfileConditions> where TProfileConditions : ConditionalProfileQuery
    {
        /// <summary>
        /// Выставить профиль на показ
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="displayed">Выставить/убрать</param>
        /// <returns></returns>
        Task<Result<TGetDto?>> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken);

        /// <summary>
        /// Получить отфильтрованный и отсортированный список всех соответствующих профилей
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ICollection<TGetDto>>> GetFiltered(TProfileConditions query, CancellationToken cancellationToken);

        /// <summary>
        /// Получить отфильтрованный и отсортированный список соответствующих профилей с пагинацией
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PaginatedResult<TGetDto>>> GetPaginated(TProfileConditions query, uint page, uint pageSize, CancellationToken cancellationToken);
    }
}
