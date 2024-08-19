using AutoMapper;
using Common.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories.Models;
using Service.Contracts.Player;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;
using static Common.Models.ConditionalQuery;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamBoardService(IMapper mapper, ITeamRepository teamRepo) : IBoardService<TeamDto, TeamConditions>
    {
        public Task<ICollection<PlayerDto>> GetFiltered(TeamConditions query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<PlayerDto>> GetPaginated(TeamConditions query, int page, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TeamDto?> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken)
        {
            var team = new Team() { Id = id, Displayed = displayed };
            var updatedTeam = await teamRepo.Update(team, cancellationToken);
            return updatedTeam == null ? null : mapper.Map<TeamDto>(updatedTeam);
        }
    }
}
