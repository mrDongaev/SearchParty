using AutoMapper;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamBoardService(IMapper mapper, ITeamRepository teamRepo) : IBoardService<TeamDto>
    {
        public async Task<TeamDto> SetDisplay(Guid id, bool displayed, CancellationToken cancellationToken)
        {
            var player = await teamRepo.Get(id, cancellationToken);
            player.Displayed = displayed;
            var updatedPlayer = await teamRepo.Update(player, cancellationToken);
            return mapper.Map<TeamDto>(updatedPlayer);
        }
    }
}
