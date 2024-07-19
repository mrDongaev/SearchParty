using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Team;
using Service.Services.Interfaces;

namespace Service.Services.Implementations
{
    public class TeamService(IMapper mapper, ITeamRepository teamRepo, IPlayerRepository playerRepo) : ITeamService
    {
        public async Task<TeamDto> Create(CreateTeamDto dto, CancellationToken cancellationToken)
        {
            var team = mapper.Map<Team>(dto);
            var createdTeam = await teamRepo.Add(team, cancellationToken);
            return mapper.Map<TeamDto>(createdTeam);
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
        {
            return await teamRepo.Delete(id, cancellationToken);
        }

        public async Task<TeamDto> Get(Guid id, CancellationToken cancellationToken)
        {
            var team = await teamRepo.Get(id, cancellationToken);
            return mapper.Map<TeamDto>(team);
        }

        public async Task<ICollection<TeamDto>> GetAll(CancellationToken cancellationToken)
        {
            var teams = await teamRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<TeamDto>>(teams);
        }

        public async Task<TeamDto> Update(UpdateTeamDto dto, CancellationToken cancellationToken)
        {
            var existingTeam = await Get(dto.Id, cancellationToken);
            if (existingTeam == null)
            {
                throw new Exception($"Профиль команды с Id = {dto.Id} не найден");
            }
            var team = mapper.Map<Team>(dto);
            var updatedTeam = await teamRepo.Update(team, cancellationToken);
            return mapper.Map<TeamDto>(updatedTeam);
        }
    }
}
