using AutoMapper;
using Common.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Models;
using Library.Models.API.UserMessaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Service.Contracts.Team;
using Service.Services.Interfaces.TeamInterfaces;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamBoardService(IMapper mapper, ITeamRepository teamRepo, IServiceProvider provider) : ITeamBoardService
    {
        public async Task<TeamDto?> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken = default)
        {
            var team = new Team() { Id = id, Displayed = displayed };
            var updatedTeam = await teamRepo.Update(team, cancellationToken);
            return updatedTeam == null ? null : mapper.Map<TeamDto>(updatedTeam);
        }

        public async Task SendTeamApplicationRequest(Message message, CancellationToken cancellationToken = default)
        {
            using (var scope = provider.CreateScope())
            {
                var sender = scope.ServiceProvider.GetRequiredService<ISendEndpoint>();
                await sender.Send(message, cancellationToken);
            }
        }

        public async Task<ICollection<TeamDto>> GetFiltered(ConditionalTeamQuery query, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetConditionalTeamRange(query, cancellationToken);
            return mapper.Map<ICollection<TeamDto>>(teams);
        }

        public async Task<PaginatedResult<TeamDto>> GetPaginated(ConditionalTeamQuery query, uint page, uint pageSize, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetPaginatedTeamRange(query, page, pageSize, cancellationToken);
            return mapper.Map<PaginatedResult<TeamDto>>(teams);
        }
    }
}
