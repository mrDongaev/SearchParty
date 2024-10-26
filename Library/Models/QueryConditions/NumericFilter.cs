using Library.Models.Enums;
using System.Numerics;

namespace Library.Models.QueryConditions
{
    public sealed class NumericFilter<T> where T : INumber<T>
    {
        public T Input { get; set; }

        public NumericFilterType FilterType { get; set; }
    }
}
