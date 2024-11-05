using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.QueryConditions
{
    public sealed class DateTimeFilter
    {
        [Required]
        public DateTime DateTime { get; set; }

        public DateTimeFilterType FilterType { get; set; } = DateTimeFilterType.AtOrAfter;
    }
}
