namespace Service.Services.Interfaces.UserInterfaces
{
    public interface IUserIdentityService
    {
        Task<Guid?> GetPlayerUserId(Guid playerId, CancellationToken cancellationToken);

        Task<Guid?> GetTeamUserId(Guid teamId, CancellationToken cancellationToken);
    }
}
