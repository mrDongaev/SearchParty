using Service.Contracts.Position;

namespace Service.Services.Interfaces
{
    public interface IPositionService : IService<PositionDto, int>, IRangeGettable<PositionDto, int>
    {
    }
}
