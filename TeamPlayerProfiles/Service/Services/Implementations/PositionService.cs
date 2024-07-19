using AutoMapper;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Position;
using Service.Services.Interfaces;

namespace Service.Services.Implementations
{
    public class PositionService(IMapper mapper, IPositionRepository posRepo) : IPositionService
    {
        public async Task<PositionDto> Get(int id, CancellationToken cancellationToken = default)
        {
            var position = await posRepo.Get(id, cancellationToken);
            return mapper.Map<PositionDto>(position);
        }

        public async Task<ICollection<PositionDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var positions = await posRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<PositionDto>>(positions);
        }
    }
}
