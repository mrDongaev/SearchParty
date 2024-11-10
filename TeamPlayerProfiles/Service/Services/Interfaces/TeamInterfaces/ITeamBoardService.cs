using Common.Models;
using Library.Models.API.UserMessaging;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.TeamInterfaces
{
    public interface ITeamBoardService : IBoardService<TeamDto, ConditionalTeamQuery>
    {
        Task SendTeamApplicationRequest(Message message, CancellationToken cancellationToken);
    }
}
