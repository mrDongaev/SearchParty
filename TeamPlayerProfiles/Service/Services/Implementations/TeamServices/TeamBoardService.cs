using AutoMapper;
using Common.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Models;
using Library.Models.API.UserProfiles.User;
using Library.Models.Enums;
using Library.Models.QueryConditions;
using Service.Contracts.Team;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Interfaces.UserProfilesInterfaces;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamBoardService(IMapper mapper, ITeamRepository teamRepo, IUserProfileService userService) : ITeamBoardService
    {
        public async Task<TeamDto?> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken)
        {
            var team = new Team() { Id = id, Displayed = displayed };
            var updatedTeam = await teamRepo.Update(team, cancellationToken);
            return updatedTeam == null ? null : mapper.Map<TeamDto>(updatedTeam);
        }

        public async Task SendTeamAccessionRequest(Guid teamId, Guid requestingPlayerId, CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            throw new NotImplementedException();
        }

        public async Task<ICollection<TeamDto>> GetFiltered(ConditionalTeamQuery query, CancellationToken cancellationToken)
        {
            var teams = await teamRepo.GetConditionalTeamRange(query, cancellationToken);
            return await AddUserInfo(teams, query.MinMmr, query.MaxMmr, cancellationToken);
        }

        public async Task<PaginatedResult<TeamDto>> GetPaginated(ConditionalTeamQuery query, uint page, uint pageSize, CancellationToken cancellationToken)
        {
            var teams = await teamRepo.GetPaginatedTeamRange(query, page, pageSize, cancellationToken);
            var filteredTeams = await AddUserInfo(teams.List, query.MinMmr, query.MaxMmr, cancellationToken);

            teams.List = new List<Team>();
            var paginatedTeams = mapper.Map<PaginatedResult<TeamDto>>(teams);
            paginatedTeams.List = filteredTeams;

            return paginatedTeams;
        }

        private async Task<ICollection<TeamDto>> AddUserInfo(ICollection<Team> teams, NumericFilter<uint>? minMmrFilter, NumericFilter<uint>? maxMmrFilter, CancellationToken cancellationToken)
        {
            var userIds = teams.SelectMany(t => t.TeamPlayers).Select(tp => tp.PlayerUserId).ToList();
            var teamDtos = mapper.Map<ICollection<TeamDto>>(teams);

            GetConditionalUser.Request request = new()
            {
                MaxMmr = maxMmrFilter,
                MinMmr = minMmrFilter,
                UserIDs = new ValueListFilter<Guid>
                {
                    FilterType = ValueListFilterType.Including,
                    ValueList = userIds,
                },
            };
            var users = await userService.GetFiltered(request, cancellationToken);
            var filteredTeamDtos = new List<TeamDto>();
            foreach (var team in teamDtos)
            {
                bool mmrCompliant = true;
                foreach (var teamPlayer in team.PlayersInTeam)
                {
                    var player = teamPlayer.Player;
                    var user = users.SingleOrDefault(u => u.Id == player.UserId);
                    if (user == null)
                    {
                        mmrCompliant = false;
                        break;
                    }
                    player.Mmr = user.Mmr;
                }
                if (!mmrCompliant) continue;
                filteredTeamDtos.Add(team);
            }
            return filteredTeamDtos;
        }
    }
}
