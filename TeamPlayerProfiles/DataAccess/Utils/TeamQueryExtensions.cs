using Common.Models;
using DataAccess.Entities;
using Library.Exceptions;
using Library.Models.Enums;
using Library.Models.QueryConditions;
using Library.Repositories.Utils;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Utils
{
    public static class TeamQueryExtensions
    {
        public static IQueryable<Team> FilterWith(this IQueryable<Team> query, ConditionalTeamQuery queryConfig)
        {
            var finalLambda = new QueryFilteringExpressionBuilder<Team>("team")
                .ApplyStringFiltering(queryConfig.NameFilter, "Name")
                .ApplyStringFiltering(queryConfig.DescriptionFilter, "Description")
                .ApplySingleValueFiltering(queryConfig.DisplayedFilter, "Displayed")
                .ApplyDateTimeFiltering(queryConfig.UpdatedAtStart, "UpdatedAt")
                .ApplyDateTimeFiltering(queryConfig.UpdatedAtEnd, "UpdatedAt")
                .ApplyNumericFiltering(queryConfig.PlayerCountStart, "PlayerCount")
                .ApplyNumericFiltering(queryConfig.PlayerCountEnd, "PlayerCount")
                .BuildLambdaExpression();

            if (queryConfig.PositionFilter != null)
            {
                int count = queryConfig.PositionFilter.ValueList.Count;
                ValueListFilter<int> includingFilter = new ValueListFilter<int>
                {
                    FilterType = ValueListFilterType.Including,
                    ValueList = queryConfig.PositionFilter.ValueList,
                };
                var teamPlayerLambda = new QueryFilteringExpressionBuilder<TeamPlayer>("teamPlayer")
                        .ApplyValueListFiltering(includingFilter, "PositionId")
                        .BuildLambdaExpression();
                query = queryConfig.PositionFilter.FilterType switch
                {
                    ValueListFilterType.Exact => query.Where(t => t.TeamPlayers.Count() == count && 
                                                        t.TeamPlayers.AsQueryable()
                                                            .Where(teamPlayerLambda)
                                                            .Count() == count),

                    ValueListFilterType.Including => query.Where(t => t.TeamPlayers.AsQueryable()
                                                        .Where(teamPlayerLambda)
                                                        .Count() == count),

                    ValueListFilterType.Excluding => query.Where(t => t.TeamPlayers.AsQueryable()
                                                        .Where(teamPlayerLambda)
                                                        .Count() == 0),

                    ValueListFilterType.Any => query.Where(t => t.TeamPlayers.AsQueryable()
                                                        .Where(teamPlayerLambda)
                                                        .Count() > 0),

                    _ => throw new InvalidEnumMemberException($"{queryConfig.PositionFilter.FilterType}", typeof(ValueListFilterType).Name),
                };
            }
            if (queryConfig.HeroFilter != null)
            {
                int count = queryConfig.HeroFilter.ValueList.Count;
                ValueListFilter<int> includingFilter = new ValueListFilter<int>
                {
                    FilterType = ValueListFilterType.Including,
                    ValueList = queryConfig.HeroFilter.ValueList,
                };
                var heroLambda = new QueryFilteringExpressionBuilder<Hero>("hero")
                    .ApplyValueListFiltering(includingFilter, "Id")
                    .BuildLambdaExpression();
                query = queryConfig.HeroFilter.FilterType switch
                {
                    ValueListFilterType.Exact => query.Where(t => t.TeamPlayers.AsQueryable()
                                                        .SelectMany(tp => tp.Player.Heroes)
                                                        .Count() == count &&
                                                            t.TeamPlayers.AsQueryable()
                                                                .SelectMany(tp => tp.Player.Heroes)
                                                                .AsQueryable()
                                                                .Where(heroLambda)
                                                                .Count() == count),

                    ValueListFilterType.Including => query.Where(t => t.TeamPlayers.AsQueryable()
                                                        .SelectMany(tp => tp.Player.Heroes)
                                                        .AsQueryable()
                                                        .Where(heroLambda)
                                                        .Count() == count),

                    ValueListFilterType.Excluding => query.Where(t => t.TeamPlayers.AsQueryable()
                                                        .SelectMany(tp => tp.Player.Heroes)
                                                        .AsQueryable()
                                                        .Where(heroLambda).Count() == 0),

                    ValueListFilterType.Any => query.Where(t => t.TeamPlayers.AsQueryable()
                                                        .SelectMany(tp => tp.Player.Heroes)
                                                        .AsQueryable()
                                                        .Where(heroLambda).Count() > 0),

                    _ => throw new InvalidEnumMemberException($"{queryConfig.HeroFilter.FilterType}", typeof(ValueListFilterType).Name),
                };
            }
            return query.Where(finalLambda);
        }

        public static IQueryable<Team> SortWith(this IQueryable<Team> query, SortCondition? sortConfig)
        {
            query = query.OrderBy(t => t.Id);
            if (sortConfig == null) return query;
            var builder = new QuerySortingExpressionBuilder<Team>(query)
                .ApplySort(sortConfig);
            return builder.GetSortedQuery();
        }

        public static IQueryable<Team> SortWith(this IQueryable<Team> query, ICollection<SortCondition>? sortConfig)
        {
            query = query.OrderBy(t => t.Id);
            if (sortConfig == null) return query;
            var builder = new QuerySortingExpressionBuilder<Team>(query)
                .ApplySort(sortConfig);
            return builder.GetSortedQuery();
        }

        public static IQueryable<Team> GetEntities(this IQueryable<Team> query, bool asNoTracking)
        {
            query = asNoTracking ? query.AsNoTracking() : query;
            return query
                .Include(t => t.TeamPlayers)
                .ThenInclude(p => p.Player)
                .ThenInclude(p => p.User)
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
