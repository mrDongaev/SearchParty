using Common.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Common.Models.ConditionalQuery.Filter;

namespace Common.Models
{
    public static class ConditionalQuery
    {
        public abstract class ProfileConditions
        {
            public StringFilter? NameFilter { get; set; }

            public StringFilter? DescriptionFilter { get; set; }

            public SingleValueFilter<bool>? DisplayedFilter { get; set; }

            public TimeFilter? UpdatedAtStart { get; set; }

            public TimeFilter? UpdatedAtEnd { get; set; }

            public Sort? Sort { get; set; }
        }

        public sealed class TeamConditions : ProfileConditions
        {

        }

        public sealed class PlayerConditions : ProfileConditions
        {
            public ValueFilter<int>? PositionFilter { get; set; }

            public ValueFilter<int>? HeroFilter { get; set; }
        }

        public static class Filter
        {
            public sealed class TimeFilter
            {
                [Required]
                public DateTime DateTime { get; set; }

                [Required]
                public DateTimeFilter FilterType { get; set; }
            }

            public sealed class StringFilter
            {
                [Required]
                public string Input { get; set; }

                [Required]
                public StringValueFilterType FilterType { get; set; }
            }

            public sealed class ValueFilter<T>
            {
                [Required]
                public ICollection<T> ValueList { get; set; }

                [Required]
                public ValueListFilterType FilterType { get; set; }
            }
            
            public sealed class SingleValueFilter<T>
            {
                [Required]
                public T Value { get; set; }

                [Required]
                public SingleValueFilterType FilterType { get; set; }
            }
        }

        public sealed class Sort
        {
            [Required]
            public string SortBy { get; set; }

            [Required]
            public SortDirection SortDirection { get; set; } = SortDirection.Asc;
        }
    }
}
