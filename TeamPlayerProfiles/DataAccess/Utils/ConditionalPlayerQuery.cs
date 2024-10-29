using DataAccess.Entities;
using Library.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Common.Models.ConditionalProfileQuery;

namespace DataAccess.Utils
{
    public static class ConditionalPlayerQuery
    {
        public static IQueryable<Player> FilterWith(this IQueryable<Player> query, PlayerConditions queryConfig)
        {
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

        public static IQueryable<Player> GetEntities(this IQueryable<Player> query, bool asNoTracking)
        {
            var first = asNoTracking ? query.AsNoTracking() : query;
            return first
                .Include(p => p.Heroes)
                .Include(p => p.Position);
        }
    }



}
