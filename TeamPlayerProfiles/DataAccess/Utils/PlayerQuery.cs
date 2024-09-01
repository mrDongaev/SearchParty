using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Common.Models.ConditionalQuery;

namespace DataAccess.Utils
{
    public static class PlayerQuery
    {
        public static IQueryable<Player> FilterWith(this IQueryable<Player> query, PlayerConditions queryConfig)
        {
            var parameter = Expression.Parameter(typeof(Player), "player");
            Expression expr = Expression.Constant(true)
                .GetStringFilteringExpression<Player>(queryConfig.NameFilter, "Name", parameter)
                .GetStringFilteringExpression<Player>(queryConfig.DescriptionFilter, "Description", parameter)
                .GetSingleValueFilteringExpression<Player, bool?>(queryConfig.DisplayedFilter, "Displayed", parameter)
                .GetDateTimeFilteringExpression<Player>(queryConfig.UpdatedAtStart, "UpdatedAt", parameter)
                .GetDateTimeFilteringExpression<Player>(queryConfig.UpdatedAtEnd, "UpdatedAt", parameter)
                .GetValueListFilteringExpression<Player, int?>(queryConfig.PositionFilter, "PositionId", parameter)
                .GetValueListOnMemberListFilteringExpression<Player, Hero, int>(queryConfig.HeroFilter, "Heroes", "Id", parameter);
            Expression<Func<Player, bool>> finalLambda = Expression.Lambda<Func<Player, bool>>(expr, parameter);
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
