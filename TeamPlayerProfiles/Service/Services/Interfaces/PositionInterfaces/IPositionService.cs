using Library.Services.Interfaces;
using Service.Contracts.Position;

namespace Service.Services.Interfaces.PositionInterfaces
{
    public interface IPositionService : IService<PositionDto, int>, IRangeGettable<PositionDto, int>
    {
    }
}
