﻿using FluentResults;
using Library.Services.Interfaces;
using Service.Contracts.User;

namespace Service.Services.Interfaces
{
    public interface IUserService : IService<UserDto, Guid>, IRangeGettable<UserDto, Guid>
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
        Task<Result<UserDto?>> Create(CreateUserDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Обновить данные профиля
        /// </summary>
        /// <param name="dto">ДТО обновляемого профиля</param>
        /// <returns>ДТО обновлённого профиля</returns>
        Task<Result<UserDto?>> Update(UpdateUserDto dto, CancellationToken cancellationToken);
    }
}
