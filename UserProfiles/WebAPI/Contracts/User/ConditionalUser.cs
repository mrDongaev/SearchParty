using Library.Models.Enums;
using Library.Models.QueryConditions;

namespace WebAPI.Contracts.User
{
    public static class ConditionalUser
    {
        public sealed class Request
        {
            public NumericFilter<int>? MinMmr { get; set; } = new NumericFilter<int> () { FilterType = NumericFilterType.GreaterOrEqual, Input = 0 };

            public NumericFilter<int>? MaxMmr { get; set; } = new NumericFilter<int>() { FilterType = NumericFilterType.LessOrEqual, Input = 20000 };

            public SortDirection SortDirection { get; set; } = SortDirection.Asc;
        }
    }
}
