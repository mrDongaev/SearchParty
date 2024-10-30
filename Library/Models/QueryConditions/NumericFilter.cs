using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Library.Models.QueryConditions
{
    public sealed class NumericFilter<T> where T : INumber<T>
    {
        [Required]
        public T Input { get; set; }

        public NumericFilterType FilterType { get; set; } = NumericFilterType.GreaterOrEqual;
    }
}
