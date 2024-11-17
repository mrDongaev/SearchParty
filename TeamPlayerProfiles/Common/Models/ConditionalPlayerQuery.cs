using Library.Models.QueryConditions;

namespace Common.Models
{
    public class ConditionalPlayerQuery : ConditionalProfileQuery
    {
        public ValueListFilter<int?>? PositionFilter { get; set; }

        public ValueListFilter<int>? HeroFilter { get; set; }
    }
}
