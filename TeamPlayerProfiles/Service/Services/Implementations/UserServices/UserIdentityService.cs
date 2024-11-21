using Microsoft.Extensions.DependencyInjection;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Interfaces.UserInterfaces;

namespace Service.Services.Implementations.UserServices
{
    public class UserIdentityService(IServiceProvider serviceProvider) : IUserIdentityService
    {
        public async Task<Guid?> GetPlayerUserId(Guid playerId, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            using (var scope = serviceProvider.CreateScope())
            {
                var playerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                userId = await playerService.GetProfileUserId(playerId, cancellationToken);
            }
            return userId;
        }

        public async Task<Guid?> GetTeamUserId(Guid teamId, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            using (var scope = serviceProvider.CreateScope())
            {
                var playerService = scope.ServiceProvider.GetRequiredService<ITeamService>();
                userId = await playerService.GetProfileUserId(teamId, cancellationToken);
            }
            return userId;
        }
    }
}
