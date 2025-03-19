using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using FluentResults;
using Library.Models.API.UserProfiles.User;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Contracts.User;
using Service.Services.Interfaces;
using Service.Services.Interfaces.UserProfilesInterfaces;

namespace Service.Services.Implementations
{
    public class UserService(IMapper mapper, IUserRepository userRepo, IUserHttpContext userContext, IUserProfileService userProfileService) : IUserService
    {
        public async Task<Result<UserDto?>> Create(CreateUserDto dto, CancellationToken cancellationToken)
        {
            var user = await userRepo.Get(dto.Id, cancellationToken);
            if (user == null)
            {
                return Result.Fail<UserDto?>(new EntityAlreadyExistsError("User with the given ID already exists"));
            }
            if (dto.Id != userContext.UserId)
            {
                return Result.Fail<UserDto?>(new UnauthorizedError());
            }
            var newUser = mapper.Map<User>(dto);
            var createdUser = await userRepo.Add(newUser, cancellationToken);
            GetUser.Response? userProfile = null;
            if (createdUser != null)
            {
                CreateUser.Request userRequest = mapper.Map<CreateUser.Request>(dto);
                var result = await userProfileService.Create(userRequest, cancellationToken);
                if (result.IsFailed) return result.Map<UserDto?>(res => null);
                userProfile = result.Value;
            }
            var userDto = mapper.Map<UserDto?>(createdUser);
            userDto.Mmr = userProfile.Mmr;
            return Result.Ok<UserDto?>(userDto);

        }

        public async Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken)
        {
            var profile = await userRepo.Get(id, cancellationToken);
            if (profile == null)
            {
                return Result.Fail<bool>(new EntityNotFoundError("User profile with the given ID has not been found"));
            }
            if (profile.Id != userContext.UserId)
            {
                return Result.Fail<bool>(new UnauthorizedError());
            }
            var result = await userProfileService.Delete(id, cancellationToken);
            if (result.IsSuccess)
            {
                var response = await userRepo.Delete(id, cancellationToken);
                if (response)
                {
                    return Result.Ok(response);
                }
                else
                {
                    return Result.Fail<bool>(new EntityNotFoundError("User profile with the given ID has not been found"));
                }
            }
            else
            {
                return result.Map<bool>(res => false);
            }

        }

        public async Task<Result<UserDto?>> Get(Guid id, CancellationToken cancellationToken)
        {
            var user = await userRepo.Get(id, cancellationToken);
            var result = await userProfileService.Get(id, cancellationToken);
            if (result.IsSuccess)
            {
                if (result.IsFailed)
                {
                    return result.Map<UserDto?>(res => null);
                }

            }
            if (user != null)
            {
                var userDto = mapper.Map<UserDto>(user);
                userDto.Mmr = result.Value.Mmr;
                return Result.Ok<UserDto?>(userDto);
            }
            else
            {
                return Result.Fail<UserDto?>(new EntityNotFoundError("User profile with the given ID has not been found"));
            }
        }

        public async Task<Result<ICollection<UserDto>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await userRepo.GetAll(cancellationToken);
            var result = await userProfileService.GetAll(cancellationToken);
            if (result.IsFailed)
            {
                return result.Map<ICollection<UserDto>>(res => []);
            }
            var userProfiles = result.Value;
            if (users.Any() && userProfiles.Any())
            {
                var userDtos = mapper.Map<ICollection<UserDto>>(users);
                foreach (var user in userDtos)
                {
                    var userProfile = userProfiles.SingleOrDefault(u => u.Id == user.Id);
                    if (userProfile != null) user.Mmr = userProfile.Mmr;
                }
                return Result.Ok(userDtos);
            }
            else
            {
                return Result.Fail<ICollection<UserDto>>(new EntitiesNotFoundError("No user profiles have been found."));
            }
        }

        public async Task<Result<ICollection<UserDto>>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var users = await userRepo.GetRange(ids, cancellationToken);
            var result = await userProfileService.GetRange(ids, cancellationToken);
            if (result.IsFailed)
            {
                return result.Map<ICollection<UserDto>>(res => []);
            }
            var userProfiles = result.Value;
            if (users.Any() && userProfiles.Any())
            {
                var userDtos = mapper.Map<ICollection<UserDto>>(users);
                foreach (var user in userDtos)
                {
                    var userProfile = userProfiles.SingleOrDefault(u => u.Id == user.Id);
                    if (userProfile != null) user.Mmr = userProfile.Mmr;
                }
                return Result.Ok(userDtos);
            }
            else
            {
                return Result.Fail<ICollection<UserDto>>(new EntitiesNotFoundError("No user profiles have been found."));
            }
        }

        public async Task<Result<UserDto?>> Update(UpdateUserDto dto, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(dto);
            var updatedUser = await userRepo.Update(user, cancellationToken);
            GetUser.Response? userProfile = null;
            if (updatedUser != null)
            {
                var result = await userProfileService.Update(dto.Id, mapper.Map<UpdateUser.Request>(dto), cancellationToken);
                if (result.IsFailed)
                {
                    return result.Map<UserDto?>(res => null);
                }
                userProfile = result.Value;
            }
            var userDto = mapper.Map<UserDto>(updatedUser);
            userDto.Mmr = userProfile.Mmr;
            return Result.Ok<UserDto?>(userDto);
        }
    }
}
