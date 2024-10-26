using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.QueryConditions
{
    public sealed class SortCondition
    {
        [Required]
        public string SortBy { get; set; }

        [Required]
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    }
}
