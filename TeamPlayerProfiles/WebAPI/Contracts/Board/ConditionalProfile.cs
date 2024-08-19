using System.ComponentModel.DataAnnotations;
using WebAPI.Contracts.Interfaces;
using static Common.Models.ConditionalQuery;
using static Common.Models.ConditionalQuery.Filter;

namespace WebAPI.Contracts.Board
{
    public static class ConditionalProfile
    {
        public abstract class Request
        {
            public StringFilter? NameFilter { get; set; }

            public StringFilter? DescriptionFilter { get; set; }

            public SingleValueFilter<bool>? DisplayedFilter { get; set; }

            public TimeFilter? UpdatedAtStart { get; set; }

            public TimeFilter? UpdatedAtEnd { get; set; }

            public Sort? Sort { get; set; }
        }

        public class PlayerRequest : Request
        {
            public ValueFilter<int>? PositionFilter { get; set; }

            public ValueFilter<int>? HeroFilter { get; set; }
        }

        public class PaginatedPlayerRequest : Request, IPaginatable
        {
            public ValueFilter<int>? PositionFilter { get; set; }

            public ValueFilter<int>? HeroFilter { get; set; }

            public int? Page { get; set; } = 1;

            public int? PageSize { get; set; } = 10;
        }

        public sealed class TeamRequest : Request
        {

        }
    }
}
