using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class UserRepository : Repository<UserProfilesContext, User, Guid>, IUserRepository
    {
        private readonly DbSet<User> _users;

        public UserRepository(UserProfilesContext context) : base(context)
        {
            _users = context.Users;
        }

        public override async Task<User?> Update(User user, CancellationToken cancellationToken)
        {
            var existingUser = await _users.SingleOrDefaultAsync(p => p.Id == user.Id, cancellationToken);
            if (existingUser == null)
            {
                return null;
            }
            if (user.Name != null) existingUser.Name = user.Name;
            if (user.Description != null) existingUser.Description = user.Description;
            if (user.SteamFriendCode != null) existingUser.SteamFriendCode = user.SteamFriendCode;
            if (user.DiscordName != null) existingUser.DiscordName = user.DiscordName;
            if (user.TelegramLink != null) existingUser.TelegramLink = user.TelegramLink;
            if (_context.Entry(existingUser).State == EntityState.Modified)
            {
                existingUser.UpdatedAt = DateTime.UtcNow;
            }
            var updatedUser = await base.Update(existingUser, cancellationToken);
            return updatedUser;
        }
    }
}
