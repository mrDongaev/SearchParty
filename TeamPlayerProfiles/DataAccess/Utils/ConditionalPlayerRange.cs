using Common.Models.Enums;
using DataAccess.Entities;
using DataAccess.Entities.Interfaces;
using System.Linq.Expressions;
using static Common.Models.ConditionalQuery;
using static Common.Models.ConditionalQuery.Filter;
using static DataAccess.Utils.ProfileQueryExpression;

namespace DataAccess.Utils
{
    public static class ConditionalPlayerRange
    {
        public static IQueryable<Player> FilterWith(this IQueryable<Player> query, PlayerConditions queryConfig)
        {
            var parameter = Expression.Parameter(typeof(Player), "profile");
            Expression expr = Expression.Constant(true);
            expr.GetStringFilteringExpression<Player>(queryConfig.NameFilter, "Name", parameter)
                .GetStringFilteringExpression<Player>(queryConfig.DescriptionFilter, "Description", parameter)
                .GetValueListFilteringExpression<Player, int?>(queryConfig.PositionFilter, "PositionId", parameter)
                .GetValueListFilteringOnListExpression<Player, Hero, int>(queryConfig.HeroFilter, "Heroes", "Id", parameter)
                .GetSingleValueFilteringExpression<Player, bool?>(queryConfig.DisplayedFilter, "Displayed", parameter)
                .GetDateTimeFilteringExpression<Player>(queryConfig.UpdatedAtStart, "UpdatedAt", parameter)
                .GetDateTimeFilteringExpression<Player>(queryConfig.UpdatedAtEnd, "UpdatedAt", parameter);
            Expression<Func<Player, bool>> finalLambda = Expression.Lambda<Func<Player, bool>>(expr, parameter);
            return query.Where(finalLambda);
        }
    }



}
