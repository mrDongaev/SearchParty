using Common.Models.Enums;
using DataAccess.Entities;
using DataAccess.Entities.Interfaces;
using System.Linq.Expressions;
using static Common.Models.ConditionalQuery;
using static Common.Models.ConditionalQuery.Filter;
using static DataAccess.Utils.ProfileQueryExpression;

namespace DataAccess.Repositories.Models
{
    public static class ConditionalPlayerRange
    {
        public static IQueryable<Player> FilterWith(this IQueryable<Player> query, PlayerConditions queryConfig)
        {
            var parameter = Expression.Parameter(typeof(Player), "profile");
            Expression expr = Expression.Constant(true);
            if (queryConfig.NameFilter != null)
            {
                var condExpr = GetStringFilteringExpression<Player>(queryConfig.NameFilter, "Name", parameter);
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.DescriptionFilter != null)
            {
                var condExpr = GetStringFilteringExpression<Player>(queryConfig.DescriptionFilter, "Description", parameter);
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.PositionFilter != null)
            {
                var condExpr = GetValueListFilteringExpression<Player, int?>(queryConfig.PositionFilter, "PositionId", parameter);
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.HeroFilter != null)
            {
                var condExpr = GetValueListFilteringOnListExpression<Player, Hero, int>(queryConfig.HeroFilter, "Heroes", "Id", parameter);
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.DisplayedFilter != null)
            {
                var condExpr = GetSingleValueFilteringExpression<Player, bool?>(queryConfig.DisplayedFilter, "Displayed", parameter);
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.UpdatedAtStart != null)
            {
                var condExpr = GetDateTimeFilteringExpression<Player>(queryConfig.UpdatedAtStart, "UpdatedAt", parameter);
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.UpdatedAtEnd != null)
            {
                var condExpr = GetDateTimeFilteringExpression<Player>(queryConfig.UpdatedAtEnd, "UpdatedAt", parameter);
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            Expression<Func<Player, bool>> finalLambda = Expression.Lambda<Func<Player, bool>>(expr, parameter);
            return query.Where(finalLambda);
        }
    }



}
