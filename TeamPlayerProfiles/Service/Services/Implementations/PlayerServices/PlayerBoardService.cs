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
    public class PlayerBoardService(IMapper mapper, IPlayerRepository playerRepo, IUserProfileService userService) : IPlayerBoardService
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
            var mmrSortCondition = query.SortConditions.SingleOrDefault(q => q.SortBy == nameof(PlayerDto.Mmr));
            var filteredPlayerDtos = await AddUserInfo(players, query.MinMmr, query.MaxMmr, mmrSortCondition, cancellationToken);
            return filteredPlayerDtos;
        }

        public async Task<PaginatedResult<PlayerDto>> GetPaginated(ConditionalPlayerQuery query, uint page, uint pageSize, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetPaginatedPlayerRange(query, page, pageSize, cancellationToken);
            var mmrSortCondition = query.SortConditions.SingleOrDefault(q => q.SortBy == nameof(PlayerDto.Mmr));
            var filteredPlayerDtos = await AddUserInfo(players.List, query.MinMmr, query.MaxMmr, mmrSortCondition, cancellationToken);

            players.List = new List<Player>();
            var paginated = mapper.Map<PaginatedResult<PlayerDto>>(players);
            paginated.List = filteredPlayerDtos;

            return paginated;
        }

        private async Task<ICollection<PlayerDto>> AddUserInfo(ICollection<Player> players, NumericFilter<uint>? minMmrFilter, NumericFilter<uint>? maxMmrFilter, SortCondition? sortCondition, CancellationToken cancellationToken)
        {
            var userIds = players.Select(p => p.UserId).ToList();
            var playerDtos = mapper.Map<ICollection<PlayerDto>>(players);
            GetConditionalUser.Request request = new()
            {
                MinMmr = minMmrFilter,
                MaxMmr = maxMmrFilter,
                UserIDs = new ValueListFilter<Guid>
                {
                    FilterType = ValueListFilterType.Including,
                    ValueList = userIds,
                },
                SortCondition = sortCondition == null ? null : [sortCondition],
            };
            var users = await userService.GetFiltered(request, cancellationToken);

            var filteredPlayerDtos = new List<PlayerDto>();
            foreach (var user in users)
            {
                var player = playerDtos.SingleOrDefault(p => p.UserId == user.Id);
                if (user == null) continue;
                player.Mmr = user.Mmr;
                filteredPlayerDtos.Add(player);
            }

            return filteredPlayerDtos;
        }
    }
}
