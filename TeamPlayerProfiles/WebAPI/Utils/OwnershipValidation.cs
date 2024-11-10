using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.TeamInterfaces;

namespace WebAPI.Utils
{
    public static class OwnershipValidation
    {
        public static async Task<bool> OwnsPlayer(IServiceProvider serviceProvider, Guid contextUserId, Guid playerId, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            using (var scope = serviceProvider.CreateScope())
            {
                var playerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
                userId = await playerService.GetProfileUserId(playerId, cancellationToken);
            }
            return userId != null && userId == contextUserId;
        }

        public static async Task<bool> OwnsTeam(IServiceProvider serviceProvider, Guid contextUserId, Guid teamId, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            using (var scope = serviceProvider.CreateScope())
            {
                var playerService = scope.ServiceProvider.GetRequiredService<ITeamService>();
                userId = await playerService.GetProfileUserId(teamId, cancellationToken);
            }
            return userId != null && userId == contextUserId;
        }
    }
}
