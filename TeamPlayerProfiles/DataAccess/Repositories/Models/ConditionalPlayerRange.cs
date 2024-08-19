using DataAccess.Entities;
using System.Linq.Expressions;
using static Common.Models.ConditionalQuery;
using static DataAccess.Utils.ProfileQueryExpression;

namespace DataAccess.Repositories.Models
{
    public static class ConditionalPlayerRange
    {
        public static IQueryable<Player> FilterWith(this IQueryable<Player> query, PlayerConditions queryConfig)
        {
            Expression expr = Expression.Equal(Expression.Constant(1), Expression.Constant(1));
            if (queryConfig.NameFilter != null)
            {
                var condExpr = GetStringFilteringExpression<Player>(queryConfig.NameFilter, "Name");
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.DescriptionFilter != null)
            {
                var condExpr = GetStringFilteringExpression<Player>(queryConfig.DescriptionFilter, "Description");
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.PositionFilter != null)
            {
                var condExpr = GetValueListFilteringExpression<Player, int>(queryConfig.PositionFilter, "PositionId");
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.HeroFilter != null)
            {
                var condExpr = GetValueListFilteringOnListExpression<Player, Hero, int>(queryConfig.HeroFilter, "Heroes", "Id");
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.DisplayedFilter != null)
            {
                var condExpr = GetSingleValueFilteringExpression<Player, bool>(queryConfig.DisplayedFilter, "Displayed");
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.UpdatedAtStart != null)
            {
                var condExpr = GetDateTimeFilteringExpression<Player>(queryConfig.UpdatedAtStart, "UpdatedAt");
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            if (queryConfig.UpdatedAtEnd != null)
            {
                var condExpr = GetDateTimeFilteringExpression<Player>(queryConfig.UpdatedAtEnd, "UpdatedAt");
                if (condExpr != null) expr = Expression.AndAlso(expr, condExpr);
            }
            var parameter = Expression.Parameter(typeof(Player), "profile");
            Expression<Func<Player, bool>> finalLambda = Expression.Lambda<Func<Player, bool>>(expr, parameter);
            return query.Where(finalLambda);
        }
    }



}
