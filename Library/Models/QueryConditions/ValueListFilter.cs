using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.QueryConditions
{
    public sealed class ValueListFilter<T>
    {
        [Required]
        public ICollection<T> ValueList { get; set; }

        public ValueListFilterType FilterType { get; set; } = ValueListFilterType.Including;
    }
}
