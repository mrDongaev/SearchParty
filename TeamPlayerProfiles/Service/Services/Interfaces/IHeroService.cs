using Service.Contracts.Hero;

namespace Service.Services.Interfaces
{
    /// <summary>
    /// Сервис героев
    /// </summary>
    public interface IHeroService : IService<HeroDto, int>, IRangeGettable<HeroDto, int>
    {
    }
}
