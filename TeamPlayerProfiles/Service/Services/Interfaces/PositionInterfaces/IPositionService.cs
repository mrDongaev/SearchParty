using Service.Contracts.Position;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.PositionInterfaces
{
    public interface IPositionService : IService<PositionDto, int>, IRangeGettable<PositionDto, int>
    {
    }
}
