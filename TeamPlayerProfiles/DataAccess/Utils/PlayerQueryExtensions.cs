using Common.Models;
using DataAccess.Entities;
using Library.Models.Enums;
using Library.Models.QueryConditions;
using Library.Repositories.Utils;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Utils
{
    public static class PlayerQueryExtensions
    {
        public static IQueryable<Player> FilterWith(this IQueryable<Player> query, ConditionalPlayerQuery? queryConfig)
        {
            if (queryConfig == null) return query;
            var builder = new QueryFilteringExpressionBuilder<Player>("player");
            var finalLambda = builder
                .ApplyStringFiltering(queryConfig.NameFilter, "Name")
                .ApplyStringFiltering(queryConfig.DescriptionFilter, "Description")
                .ApplySingleValueFiltering(queryConfig.DisplayedFilter, "Displayed")
                .ApplyDateTimeFiltering(queryConfig.UpdatedAtStart, "UpdatedAt")
                .ApplyDateTimeFiltering(queryConfig.UpdatedAtEnd, "UpdatedAt")
                .ApplyValueListFiltering(queryConfig.PositionFilter, "PositionId")
                .ApplyValueListOnMemberListFiltering<Hero, int>(queryConfig.HeroFilter, "Heroes", "Id")
                .BuildLambdaExpression();
            return query.Where(finalLambda);
        }

        public static IQueryable<Player> SortWith(this IQueryable<Player> query, SortCondition? sortConfig)
        {
            if (sortConfig == null) return query;
            var builder = new QuerySortingExpressionBuilder<Player>(query)
                .ApplySort("Id", SortDirection.Asc)
                .ApplySort(sortConfig);
            return builder.GetSortedQuery();
        }

        public static IQueryable<Player> SortWith(this IQueryable<Player> query, ICollection<SortCondition>? sortConfig)
        {
            query = query.OrderBy(p => p.Id);
            if (sortConfig == null) return query;
            var builder = new QuerySortingExpressionBuilder<Player>(query)
                .ApplySort(sortConfig);
            return builder.GetSortedQuery();
        }

        public static IQueryable<Player> GetEntities(this IQueryable<Player> query, bool asNoTracking)
        {
            var first = asNoTracking ? query.AsNoTracking() : query;
            return first
                .Include(p => p.Heroes)
                .Include(p => p.Position);
        }
    }



}
