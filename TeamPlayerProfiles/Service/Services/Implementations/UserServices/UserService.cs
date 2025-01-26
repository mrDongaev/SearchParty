using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using FluentResults;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Contracts.User;
using Service.Services.Interfaces;

namespace Service.Services.Implementations
{
    public class UserService(IMapper mapper, IUserRepository userRepo, IUserHttpContext userContext) : IUserService
    {
        public async Task<Result<UserDto?>> Create(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.Id != userContext.UserId)
            {
                return Result.Fail<UserDto?>(new UnauthorizedError()).WithValue(null);
            }
            var userExists = await userRepo.Get(dto.Id, cancellationToken) != null;
            if (userExists)
            {
                return Result.Fail<UserDto?>(new EntityAlreadyExistsError("User profile with the given ID already exists")).WithValue(null);
            }
            var newUser = mapper.Map<User>(dto);
            var createdUser = await userRepo.Add(newUser, cancellationToken);
            return Result.Ok(mapper.Map<UserDto?>(createdUser));
        }

        public async Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            if (id != userContext.UserId)
            {
                return Result.Fail<bool>(new UnauthorizedError()).WithValue(false);
            }
            var result = await userRepo.Delete(id, cancellationToken);
            if (result)
            {
                return Result.Ok(true);
            }
            return Result.Fail<bool>(new EntityNotFoundError("User profile with the given ID has not been found")).WithValue(false);
        }

        public async Task<Result<UserDto?>> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await userRepo.Get(id, cancellationToken);

            if (user == null)
            {
                return Result.Fail<UserDto?>(new EntityNotFoundError("User profile with the given ID has not been found")).WithValue(null);
            }

            return Result.Ok(mapper.Map<UserDto?>(user));
        }

        public async Task<Result<ICollection<UserDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var users = await userRepo.GetAll(cancellationToken);

            if (users.Count == 0)
            {
                return Result.Fail<ICollection<UserDto>>(new EntityListNotFoundError("No users have been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<UserDto>>(users));
        }

        public async Task<Result<ICollection<UserDto>>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken = default)
        {
            var users = await userRepo.GetRange(ids, cancellationToken);

            if (users.Count == 0)
            {
                return Result.Fail<ICollection<UserDto>>(new EntityRangeNotFoundError("User profiles with the given IDs have not been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<UserDto>>(users));
        }

        public async Task<Result<UserDto?>> Update(UpdateUserDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.Id != userContext.UserId)
            {
                return Result.Fail<UserDto?>(new UnauthorizedError()).WithValue(null);
            }
            var user = mapper.Map<User>(dto);
            var updatedUser = await userRepo.Update(user, cancellationToken);

            if (updatedUser == null)
            {
                return Result.Fail<UserDto?>(new EntityNotFoundError("User profile with the given ID has not been found")).WithValue(null);
            }

            return Result.Ok(mapper.Map<UserDto?>(updatedUser));
        }
    }
}
