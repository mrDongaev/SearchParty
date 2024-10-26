using Library.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.QueryConditions
{
    public sealed class StringFilter
    {
        [Required]
        public string Input { get; set; }

        [Required]
        public StringValueFilterType FilterType { get; set; }
    }
}
