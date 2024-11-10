using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Models.API.UserProfiles.User;
using Service.Contracts.User;
using Service.Services.Interfaces;
using Service.Services.Interfaces.UserProfilesInterfaces;

namespace Service.Services.Implementations
{
    public class UserService(IMapper mapper, IUserRepository userRepo, IUserProfileService userProfileService) : IUserService
    {
        public async Task<UserDto?> Create(CreateUserDto dto, CancellationToken cancellationToken)
        {
            var userExists = await userRepo.Get(dto.Id, cancellationToken) != null;
            if (userExists)
            {
                return null;
            }
            var newUser = mapper.Map<User>(dto);
            var createdUser = await userRepo.Add(newUser, cancellationToken);
            GetUser.Response? userProfile = null;
            if (createdUser != null)
            {
                CreateUser.Request userRequest = mapper.Map<CreateUser.Request>(dto);
                userProfile = await userProfileService.Create(userRequest, cancellationToken);
            }
            if (userProfile != null)
            {
                var userDto = mapper.Map<UserDto>(createdUser);
                userDto.Mmr = userProfile.Mmr;
                return userDto;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
        {
            var response = await userProfileService.Delete(id, cancellationToken);
            if (response)
            {
                return await userRepo.Delete(id, cancellationToken);
            } 
            else
            {
                return false;
            }
            
        }

        public async Task<UserDto?> Get(Guid id, CancellationToken cancellationToken)
        {
            var user = await userRepo.Get(id, cancellationToken);
            var userProfile = await userProfileService.Get(id, cancellationToken);
            if (user != null && userProfile != null)
            {
                var userDto = mapper.Map<UserDto>(user);
                userDto.Mmr = userProfile.Mmr;
                return userDto;
            } 
            else
            {
                return null;
            }
        }

        public async Task<ICollection<UserDto>> GetAll(CancellationToken cancellationToken)
        {
            var users = await userRepo.GetAll(cancellationToken);
            var userProfiles = await userProfileService.GetAll(cancellationToken);
            if (users.Any() && userProfiles.Any())
            {
                var userDtos = mapper.Map<ICollection<UserDto>>(users);
                foreach (var user in userDtos)
                {
                    var userProfile = userProfiles.SingleOrDefault(u => u.Id == user.Id);
                    if (userProfile != null) user.Mmr = userProfile.Mmr;
                }
                return userDtos;
            }
            else
            {
                return [];
            }
        }

        public async Task<ICollection<UserDto>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var users = await userRepo.GetRange(ids, cancellationToken);
            var userProfiles = await userProfileService.GetRange(ids, cancellationToken);
            if (users.Any() && userProfiles.Any())
            {
                var userDtos = mapper.Map<ICollection<UserDto>>(users);
                foreach (var user in userDtos)
                {
                    var userProfile = userProfiles.SingleOrDefault(u => u.Id == user.Id);
                    if (userProfile != null) user.Mmr = userProfile.Mmr;
                }
                return userDtos;
            }
            else
            {
                return [];
            }
        }

        public async Task<UserDto?> Update(UpdateUserDto dto, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(dto);
            var updatedUser = await userRepo.Update(user, cancellationToken);
            GetUser.Response? userProfile = null;
            if (updatedUser != null)
            {
                userProfile = await userProfileService.Update(dto.Id, mapper.Map<UpdateUser.Request>(dto), cancellationToken);
            }
            if (userProfile != null)
            {
                var userDto = mapper.Map<UserDto>(updatedUser);
                userDto.Mmr = userProfile.Mmr;
                return userDto;
            }
            else
            {
                return null;
            }
        }
    }
}
