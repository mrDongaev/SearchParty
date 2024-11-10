using AutoMapper;
using DataAccess.Repositories.Interfaces;
using Service.Contracts.Position;
using Service.Services.Interfaces.PositionInterfaces;

namespace Service.Services.Implementations.PositionServices
{
    public class PositionService(IMapper mapper, IPositionRepository posRepo) : IPositionService
    {
        public async Task<PositionDto?> Get(int id, CancellationToken cancellationToken = default)
        {
            var position = await posRepo.Get(id, cancellationToken);
            return position == null ? null : mapper.Map<PositionDto>(position);
        }

        public async Task<ICollection<PositionDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var positions = await posRepo.GetAll(cancellationToken);
            return mapper.Map<ICollection<PositionDto>>(positions);
        }

        public async Task<ICollection<PositionDto>> GetRange(ICollection<int> ids, CancellationToken cancellationToken = default)
        {
            var positions = await posRepo.GetRange(ids, cancellationToken);
            return mapper.Map<ICollection<PositionDto>>(positions);
        }
    }
}
