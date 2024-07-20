using Common.Models.Enums;

namespace Service.Contracts.Hero
{
    /// <summary>
    /// ДТО героя
    /// </summary>
    public class HeroDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MainStat MainStat { get; set; }
    }
}
