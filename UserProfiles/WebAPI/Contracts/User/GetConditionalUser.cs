using Library.Models.Enums;
using Library.Models.QueryConditions;

namespace WebAPI.Contracts.User
{
    public static class GetConditionalUser
    {
        public sealed class Request
        {
            public NumericFilter<uint>? MinMmr { get; set; } = new NumericFilter<uint>() { FilterType = NumericFilterType.GreaterOrEqual, Input = 0 };

            public NumericFilter<uint>? MaxMmr { get; set; } = new NumericFilter<uint>() { FilterType = NumericFilterType.LessOrEqual, Input = 20000 };

            public SortDirection SortDirection { get; set; } = SortDirection.Asc;
        }
    }
}
