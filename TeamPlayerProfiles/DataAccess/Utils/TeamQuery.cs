using Common.Models.Enums;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;
using static Common.Models.ConditionalQuery;

namespace DataAccess.Utils
{
    public static class TeamQuery
    {
        public static IQueryable<Team> FilterWith(this IQueryable<Team> query, TeamConditions queryConfig)
        {
            var parameter = Expression.Parameter(typeof(Team), "team");
            Expression expr = Expression.Constant(true)
                .GetStringFilteringExpression<Team>(queryConfig.NameFilter, "Name", parameter)
                .GetStringFilteringExpression<Team>(queryConfig.DescriptionFilter, "Description", parameter)
                .GetSingleValueFilteringExpression<Team, bool?>(queryConfig.DisplayedFilter, "Displayed", parameter)
                .GetDateTimeFilteringExpression<Team>(queryConfig.UpdatedAtStart, "UpdatedAt", parameter)
                .GetDateTimeFilteringExpression<Team>(queryConfig.UpdatedAtEnd, "UpdatedAt", parameter)
                .GetNumericFilteringExpression<Team, int>(queryConfig.PlayerCountStart, "PlayerCount", parameter)
                .GetNumericFilteringExpression<Team, int>(queryConfig.PlayerCountEnd, "PlayerCount", parameter);
            Expression<Func<Team, bool>> finalLambda = Expression.Lambda<Func<Team, bool>>(expr, parameter);
            if (queryConfig.PositionFilter != null)
            {
                int count = queryConfig.PositionFilter.ValueList.Count;
                var teamPlayerParameter = Expression.Parameter(typeof(TeamPlayer), "teamPlayer");
                Filter.ValueFilter<int> includingFilter = new Filter.ValueFilter<int>
                {
                    FilterType = ValueListFilterType.Including,
                    ValueList = queryConfig.PositionFilter.ValueList,
                };
                Expression<Func<TeamPlayer, bool>> teamPlayerLambda = Expression.Lambda<Func<TeamPlayer, bool>>(
                    Expression.Constant(true)
                        .GetValueListFilteringExpression<TeamPlayer, int>(includingFilter, "PositionId", teamPlayerParameter),
                    teamPlayerParameter);
                query = queryConfig.PositionFilter.FilterType switch
                {
                    ValueListFilterType.Exact => query.Where(t => t.TeamPlayers.Count() == count && t.TeamPlayers.AsQueryable().Where(teamPlayerLambda).Count() == count),
                    ValueListFilterType.Including => query.Where(t => t.TeamPlayers.AsQueryable()
                        .Where(teamPlayerLambda).Count() == count),
                    ValueListFilterType.Excluding => query.Where(t => t.TeamPlayers.AsQueryable()
                        .Where(teamPlayerLambda).Count() == 0),
                    ValueListFilterType.Any => query.Where(t => t.TeamPlayers.AsQueryable()
                        .Where(teamPlayerLambda).Count() > 0),
                    _ => throw new InvalidEnumArgumentException($"{queryConfig.PositionFilter.FilterType} does not exist on {typeof(ValueListFilterType).Name} type"),
                };
            }
            if (queryConfig.HeroFilter != null)
            {
                var heroParameter = Expression.Parameter(typeof(Hero), "hero");
                int count = queryConfig.HeroFilter.ValueList.Count;
                Filter.ValueFilter<int> includingFilter = new Filter.ValueFilter<int>
                {
                    FilterType = ValueListFilterType.Including,
                    ValueList = queryConfig.HeroFilter.ValueList,
                };
                Expression<Func<Hero, bool>> heroLambda = Expression.Lambda<Func<Hero, bool>>(
                    Expression.Constant(true).GetValueListFilteringExpression<Hero, int>(includingFilter, "Id", heroParameter),
                    heroParameter
                );
                query = queryConfig.HeroFilter.FilterType switch
                {
                    ValueListFilterType.Exact => query.Where(t => t.TeamPlayers.AsQueryable().SelectMany(tp => tp.Player.Heroes).Count() == count &&
                    t.TeamPlayers.AsQueryable().SelectMany(tp => tp.Player.Heroes).AsQueryable().Where(heroLambda).Count() == count),
                    ValueListFilterType.Including => query.Where(t => t.TeamPlayers.AsQueryable()
                    .SelectMany(tp => tp.Player.Heroes).AsQueryable().Where(heroLambda).Count() == count),
                    ValueListFilterType.Excluding => query.Where(t => t.TeamPlayers.AsQueryable()
                    .SelectMany(tp => tp.Player.Heroes).AsQueryable().Where(heroLambda).Count() == 0),
                    ValueListFilterType.Any => query.Where(t => t.TeamPlayers.AsQueryable()
                    .SelectMany(tp => tp.Player.Heroes).AsQueryable().Where(heroLambda).Count() > 0),
                    _ => throw new InvalidEnumArgumentException($"{queryConfig.HeroFilter.FilterType} does not exist on {typeof(ValueListFilterType).Name} type"),
                };
            }
            return query.Where(finalLambda);
        }

        public static IQueryable<Team> GetEntities(this IQueryable<Team> query, bool asNoTracking)
        {
            var first = asNoTracking ? query.AsNoTracking() : query;
            return first
                .Include(t => t.TeamPlayers)
                .ThenInclude(p => p.Player)
                .ThenInclude(p => p.Heroes)
                .Include(t => t.TeamPlayers)
                .ThenInclude(p => p.Player)
                .ThenInclude(p => p.Position)
                .Include(t => t.TeamPlayers)
                .ThenInclude(tp => tp.Position);
        }
    }
}
