using AutoMapper;
using Common.Exceptions;
using Common.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Models;
using Library.Models.API.UserMessaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Service.Contracts.Player;
using Service.Services.Interfaces.PlayerInterfaces;
using System.Reflection;

namespace Service.Services.Implementations.PlayerServices
{
    public class PlayerBoardService(IMapper mapper, IPlayerRepository playerRepo, IServiceProvider provider) : IPlayerBoardService
    {
        public async Task<PlayerDto?> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken = default)
        {
            var player = new Player() { Id = id, Displayed = displayed };
            var updatedPlayer = await playerRepo.Update(player, null, cancellationToken);
            return updatedPlayer == null ? null : mapper.Map<PlayerDto>(updatedPlayer);
        }

        public async Task InvitePlayerToTeam(ProfileMessageSubmitted message, CancellationToken cancellationToken = default)
        {
            using (var scope = provider.CreateScope())
            {
                var sender = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                var teamRepo = scope.ServiceProvider.GetRequiredService<ITeamRepository>();
                var teamPlayers = await teamRepo.GetTeamPlayers(message.SenderId, cancellationToken);
                if (teamPlayers.SingleOrDefault(tp => tp.PositionId == (int)message.PositionName) != null)
                {
                    throw new TeamPositionOverlapException();
                }
                else if (teamPlayers.SingleOrDefault(tp => tp.PlayerId == message.AcceptorId) != null)
                {
                    throw new TeamContainsPlayerException();
                }
                await sender.Publish(message, cancellationToken);
            }
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
