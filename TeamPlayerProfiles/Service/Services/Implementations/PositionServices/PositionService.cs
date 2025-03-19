using AutoMapper;
using DataAccess.Repositories.Interfaces;
using FluentResults;
using Library.Results.Errors.EntityRequest;
using Service.Contracts.Position;
using Service.Services.Interfaces.PositionInterfaces;

namespace Service.Services.Implementations.PositionServices
{
    public class PositionService(IMapper mapper, IPositionRepository posRepo) : IPositionService
    {
        public async Task<Result<PositionDto?>> Get(int id, CancellationToken cancellationToken = default)
        {
            var position = await posRepo.Get(id, cancellationToken);

            if (position == null)
            {
                return Result.Fail<PositionDto?>(new EntityNotFoundError("Position with the given ID has not been found"));
            }

            return Result.Ok(mapper.Map<PositionDto?>(position));
        }

        public async Task<Result<ICollection<PositionDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var positions = await posRepo.GetAll(cancellationToken);

            if (positions.Count == 0)
            {
                return Result.Fail<ICollection<PositionDto>>(new EntitiesNotFoundError("No positions have been found"));
            }

            return Result.Ok(mapper.Map<ICollection<PositionDto>>(positions));
        }

        public async Task<Result<ICollection<PositionDto>>> GetRange(ICollection<int> ids, CancellationToken cancellationToken = default)
        {
            var positions = await posRepo.GetRange(ids, cancellationToken);

            if (positions.Count == 0)
            {
                return Result.Fail<ICollection<PositionDto>>(new EntityRangeNotFoundError("No positions with the given IDs have been found"));
            }

            return Result.Ok(mapper.Map<ICollection<PositionDto>>(positions));
        }
    }
}
