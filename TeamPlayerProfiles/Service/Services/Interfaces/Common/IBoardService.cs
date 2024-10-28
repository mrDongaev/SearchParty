﻿using DataAccess.Repositories.Models;
using static Common.Models.ConditionalQuery;

namespace Service.Services.Interfaces.Common
{
    public interface IBoardService<TGetDto, TProfileConditions> where TProfileConditions : ProfileConditions
    {
        /// <summary>
        /// Выставить профиль на показ
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="displayed">Выставить/убрать</param>
        /// <returns></returns>
        Task<TGetDto?> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken);

        /// <summary>
        /// Получить отфильтрованный и отсортированный список всех соответствующих профилей
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<TGetDto>> GetFiltered(TProfileConditions query, CancellationToken cancellationToken);

        /// <summary>
        /// Получить отфильтрованный и отсортированный список соответствующих профилей с пагинацией
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PaginatedResult<TGetDto>> GetPaginated(TProfileConditions query, uint page, uint pageSize, CancellationToken cancellationToken);
    }
}
