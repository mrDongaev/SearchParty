using Common.Models.Enums;

namespace Service.Contracts.Position
{
    /// <summary>
    /// Позиция игрока
    /// </summary>
    public class PositionDto
    {
        public int Id { get; set; }

        public PositionName Name { get; set; }
    }
}
