﻿using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.User;
using Service.Services.Interfaces;
using Service.Services.Interfaces.Common;

namespace Service.Services.Implementations
{
    public class UserService(IMapper mapper, IUserRepository userRepo) : IUserService
    {
        public async Task<UserDto> Create(CreateUserDto dto, CancellationToken cancellationToken)
        {
            var newUser = mapper.Map<User>(dto);
            var createdUser = await userRepo.Add(newUser, cancellationToken);
            return mapper.Map<UserDto>(createdUser);
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
        {
            return await userRepo.Delete(id, cancellationToken);
        }

        public async Task<UserDto?> Get(Guid id, CancellationToken cancellationToken)
        {
            var user = await userRepo.Get(id, cancellationToken);
            return user == null ? null : mapper.Map<UserDto>(user);
        }

        public async Task<ICollection<UserDto>> GetAll(CancellationToken cancellationToken)
        {
            var users = await userRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<UserDto>>(users);
        }

        public async Task<ICollection<UserDto>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var users = await userRepo.GetRange(ids, cancellationToken);
            return mapper.Map<ICollection<UserDto>>(users);
        }

        public async Task<UserDto?> Update(UpdateUserDto dto, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(dto);
            var updatedUser = await userRepo.Update(user, cancellationToken);
            return updatedUser == null ? null : mapper.Map<UserDto>(updatedUser);
        }
    }
}
