using Library.Services.Interfaces;
using Service.Contracts.Hero;

namespace Service.Services.Interfaces.HeroInterfaces
{
    /// <summary>
    /// Сервис героев
    /// </summary>
    public interface IHeroService : IService<HeroDto, int>, IRangeGettable<HeroDto, int>
    {
    }
}
