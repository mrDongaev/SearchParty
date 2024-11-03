using Library.Models.QueryConditions;

namespace Library.Models.API.UserProfiles.User
{
    public static class GetConditionalUser
    {
        public sealed class Request
        {
            public ValueListFilter<Guid>? UserIDs { get; set; }

            public NumericFilter<uint>? MinMmr { get; set; }

            public NumericFilter<uint>? MaxMmr { get; set; }

            public ICollection<SortCondition>? SortCondition { get; set; }
        }
    }
}
