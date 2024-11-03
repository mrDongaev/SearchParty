using Common.Models;
using DataAccess.Entities;
using Library.Models.QueryConditions;
using Library.Repositories.Utils;

namespace DataAccess.Utils
{
    public static class UserQueryExtensions
    {
        public static IQueryable<User> FilterWith(this IQueryable<User> query, ConditionalUserQuery? queryConfig)
        {
            if (queryConfig == null) return query;
            var builder = new QueryFilteringExpressionBuilder<User>("user");
            var finalLambda = builder
                .ApplyValueListFiltering(queryConfig.UserIds, "Id")
                .ApplyNumericFiltering(queryConfig.MinMmr, "Mmr")
                .ApplyNumericFiltering(queryConfig.MaxMmr, "Mmr")
                .BuildLambdaExpression();
            return query.Where(finalLambda);
        }

        public static IQueryable<User> SortWith(this IQueryable<User> query, SortCondition? sortConfig)
        {
            query = query.OrderBy(u => u.Id);
            if (sortConfig == null) return query;
            var builder = new QuerySortingExpressionBuilder<User>(query)
                .ApplySort(sortConfig);
            return builder.GetSortedQuery();
        }

        public static IQueryable<User> SortWith(this IQueryable<User> query, ICollection<SortCondition>? sortConfig)
        {
            query = query.OrderBy(u => u.Id);
            if (sortConfig == null) return query;
            var builder = new QuerySortingExpressionBuilder<User>(query)
                .ApplySort(sortConfig);
            return builder.GetSortedQuery();
        }
    }
}
