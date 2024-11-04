using Common.Models;
using DataAccess.Entities;
using Library.Exceptions;
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
                .ApplyDateTimeFiltering(queryConfig.UpdatedAtFilter, "UpdatedAt")
                .ApplyDateTimeFiltering(queryConfig.UpdatedAtAddFilter, "UpdatedAt")
                .ApplyValueListFiltering(queryConfig.PositionFilter, "PositionId")
                .ApplyValueListOnMemberListFiltering<Hero, int>(queryConfig.HeroFilter, "Heroes", "Id")
                .BuildLambdaExpression();
            if (queryConfig.MmrFilter != null)
            {
                var conf = queryConfig.MmrFilter;
                query = queryConfig.MmrFilter.FilterType switch
                {
                    NumericFilterType.Equal => query.Where(p => p.User.Mmr == conf.Input),
                    NumericFilterType.NotEqual => query.Where(p => p.User.Mmr != conf.Input),
                    NumericFilterType.Less => query.Where(p => p.User.Mmr < conf.Input),
                    NumericFilterType.LessOrEqual => query.Where(p => p.User.Mmr <= conf.Input),
                    NumericFilterType.Greater => query.Where(p => p.User.Mmr > conf.Input),
                    NumericFilterType.GreaterOrEqual => query.Where(p => p.User.Mmr >= conf.Input),
                    _ => throw new InvalidEnumMemberException($"{queryConfig.MmrFilter.FilterType}", typeof(NumericFilterType).Name),
                };
            }
            if (queryConfig.MmrAddFilter != null)
            {
                var conf = queryConfig.MmrAddFilter;
                query = queryConfig.MmrAddFilter.FilterType switch
                {
                    NumericFilterType.Equal => query.Where(p => p.User.Mmr == conf.Input),
                    NumericFilterType.NotEqual => query.Where(p => p.User.Mmr != conf.Input),
                    NumericFilterType.Less => query.Where(p => p.User.Mmr < conf.Input),
                    NumericFilterType.LessOrEqual => query.Where(p => p.User.Mmr <= conf.Input),
                    NumericFilterType.Greater => query.Where(p => p.User.Mmr > conf.Input),
                    NumericFilterType.GreaterOrEqual => query.Where(p => p.User.Mmr >= conf.Input),
                    _ => throw new InvalidEnumMemberException($"{queryConfig.MmrAddFilter.FilterType}", typeof(NumericFilterType).Name),
                };
            }
            return query.Where(finalLambda);
        }

        public static IQueryable<Player> SortWith(this IQueryable<Player> query, SortCondition? sortConfig)
        {
            query = query.OrderBy(p => p.Id);
            if (sortConfig == null) return query;
            if (sortConfig.SortBy == "Mmr")
            {
                return sortConfig.SortDirection == SortDirection.Asc ? query.OrderBy(p => p.User.Mmr) : query.OrderByDescending(p => p.User.Mmr);
            } 
            else
            {
                var builder = new QuerySortingExpressionBuilder<Player>(query)
                .ApplySort(sortConfig);
                return builder.GetSortedQuery();
            }
        }

        public static IQueryable<Player> SortWith(this IQueryable<Player> query, ICollection<SortCondition>? sortConfig)
        {
            query = query.OrderBy(p => p.Id);
            if (sortConfig == null) return query;

            if (sortConfig.SingleOrDefault(c => c.SortBy == "Mmr") != null)
            {
                query = query.Select(p => new Player
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Displayed = p.Displayed,
                    UpdatedAt = p.UpdatedAt,
                    Position = p.Position,
                    PositionId = p.PositionId,
                    User = p.User,
                    UserId = p.UserId,
                    Heroes = p.Heroes,
                    PlayerHeroes = p.PlayerHeroes,
                    Teams = p.Teams,
                    TeamPlayers = p.TeamPlayers,
                    Mmr = p.User.Mmr,
                });
            }
            var builder = new QuerySortingExpressionBuilder<Player>(query)
                .ApplySort(sortConfig);
            return builder.GetSortedQuery();
        }

        public static IQueryable<Player> GetEntities(this IQueryable<Player> query, bool asNoTracking)
        {
            query = asNoTracking ? query.AsNoTracking() : query;
            return query
                .Include(p => p.Heroes)
                .Include(p => p.Position)
                .Include(p => p.User);
        }
    }
}
