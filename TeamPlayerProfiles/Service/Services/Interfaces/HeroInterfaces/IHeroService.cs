using Service.Contracts.Hero;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.HeroInterfaces
{
    /// <summary>
    /// Сервис героев
    /// </summary>
    public interface IHeroService : IService<HeroDto, int>, IRangeGettable<HeroDto, int>
    {
    }
}
