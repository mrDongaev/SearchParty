using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.QueryConditions
{
    public sealed class SingleValueFilter<T>
    {
        [Required]
        public T Value { get; set; }

        public SingleValueFilterType FilterType { get; set; } = SingleValueFilterType.Equals;
    }
}
