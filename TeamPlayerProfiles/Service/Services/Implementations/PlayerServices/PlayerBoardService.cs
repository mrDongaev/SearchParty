using AutoMapper;
using Common.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Models;
using Library.Models.API.UserProfiles.User;
using Library.Models.Enums;
using Library.Models.QueryConditions;
using Service.Contracts.Player;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Interfaces.UserProfilesInterfaces;

namespace Service.Services.Implementations.PlayerServices
{
    public class PlayerBoardService(IMapper mapper, IPlayerRepository playerRepo) : IPlayerBoardService
    {
        public async Task<PlayerDto?> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken)
        {
            var player = new Player() { Id = id, Displayed = displayed };
            var updatedPlayer = await playerRepo.Update(player, cancellationToken);
            return updatedPlayer == null ? null : mapper.Map<PlayerDto>(updatedPlayer);
        }

        public async Task InvitePlayerToTeam(Guid playerId, Guid invitingTeamId, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }

        public async Task<ICollection<PlayerDto>> GetFiltered(ConditionalPlayerQuery query, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetConditionalPlayerRange(query, cancellationToken);
            return mapper.Map<ICollection<PlayerDto>>(players);
        }

        public async Task<PaginatedResult<PlayerDto>> GetPaginated(ConditionalPlayerQuery query, uint page, uint pageSize, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetPaginatedPlayerRange(query, page, pageSize, cancellationToken);
            return mapper.Map<PaginatedResult<PlayerDto>>(players);
        }
    }
}
