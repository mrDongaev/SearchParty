using Common.Models.Enums;
using DataAccess.Entities.Interfaces;
using System.Linq.Expressions;
using static Common.Models.ConditionalQuery;
using static Common.Models.ConditionalQuery.Filter;

namespace DataAccess.Utils
{
    public static class ProfileQueryExpression
    {
        public static Expression? GetStringFilteringExpression<TProfile>(StringFilter? filter, string propertyName) where TProfile : class, IProfile
        {
            Type strType = typeof(string);
            Type profileType = typeof(TProfile);
            var property = profileType.GetProperty(propertyName);
            if (filter == null || property == null || property.PropertyType != strType) return null;
            var inputParam = Expression.Constant(filter.Input, strType);
            var parameter = Expression.Parameter(profileType, "profile");
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            var containsMethodInfo = strType.GetMethod("Contains", [strType]);
            var startsWithMethodInfo = strType.GetMethod("StartsWith", [strType]);
            var endsWithMethodInfo = strType.GetMethod("EndsWith", [strType]);
            return filter.FilterType switch
            {
                StringValueFilterType.Equals => Expression.Equal(memberAccess, inputParam),
                StringValueFilterType.DoesNotEqual => Expression.NotEqual(memberAccess, inputParam),
                StringValueFilterType.Contains => Expression.Call(memberAccess, containsMethodInfo, inputParam),
                StringValueFilterType.DoesNotContain => Expression.Not(Expression.Call(memberAccess, containsMethodInfo, inputParam)),
                StringValueFilterType.StartsWith => Expression.Call(memberAccess, startsWithMethodInfo, inputParam),
                StringValueFilterType.EndsWith => Expression.Call(memberAccess, endsWithMethodInfo, inputParam),
                _ => null,
            };
        }

        public static BinaryExpression? GetDateTimeFilteringExpression<TProfile>(TimeFilter? filter, string timePropertyName) where TProfile : class, IProfile
        {
            var property = typeof(TProfile).GetProperty(timePropertyName);
            var dateTimeType = typeof(DateTime);
            if (filter == null || property == null || property.PropertyType != dateTimeType) return null;
            var inputParam = Expression.Constant(filter.DateTime, dateTimeType);
            var parameter = Expression.Parameter(typeof(TProfile), "profile");
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            return filter.FilterType switch
            {
                DateTimeFilter.Before => Expression.LessThan(memberAccess, inputParam),
                DateTimeFilter.AtOrBefore => Expression.LessThanOrEqual(memberAccess, inputParam),
                DateTimeFilter.Exact => Expression.Equal(memberAccess, inputParam),
                DateTimeFilter.AtOrAfter => Expression.GreaterThanOrEqual(memberAccess, inputParam),
                DateTimeFilter.After => Expression.GreaterThan(memberAccess, inputParam),
                _ => null,
            };
        }

        public static Expression? GetValueListFilteringExpression<TProfile, TListItemProp>(ValueFilter<TListItemProp> filter, string valuePropertyName) where TProfile : class, IProfile
        {
            var propertyType = typeof(TListItemProp);
            var property = typeof(TProfile).GetProperty(valuePropertyName);
            if (filter == null || property == null || property.PropertyType != propertyType) return null;
            var valueList = Expression.Constant(filter.ValueList, typeof(IEnumerable<TListItemProp>));
            var parameter = Expression.Parameter(typeof(TProfile), "profile");
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .Where(x => x.Name == "Contains")
                .Single(x => x.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TListItemProp));
            Expression? finalExpression = null;
            switch (filter.FilterType)
            {
                case ValueListFilterType.Exact:
                    {
                        if (filter.ValueList.Count > 1) break;
                        finalExpression = Expression.Call(containsMethod, valueList, memberAccess);
                        break;
                    }
                case ValueListFilterType.Any:
                case ValueListFilterType.Including:
                    {
                        finalExpression = Expression.Call(containsMethod, valueList, memberAccess);
                        break;
                    }
                case ValueListFilterType.Excluding:
                    {
                        finalExpression = Expression.Not(Expression.Call(containsMethod, valueList, memberAccess));
                        break;
                    }
                default:
                    break;
            }
            return finalExpression;
        }

        public static Expression? GetValueListFilteringOnListExpression<TProfile, TListItem, TListItemProp>(ValueFilter<TListItemProp>? filter, string listPropertyName, string listItemPropertyName) where TProfile : class, IProfile
        {
            var listType = typeof(IEnumerable<TListItem>);
            var property = typeof(TProfile).GetProperty(listPropertyName);
            if (filter == null || property == null || property.PropertyType != listType) return null;
            var listItemType = typeof(TListItem);
            var valueList = Expression.Constant(filter.ValueList, typeof(IEnumerable<TListItemProp>));
            var parameter = Expression.Parameter(typeof(TProfile), "profile");
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            var intersectByMethodInfo = typeof(Enumerable)
                .GetMethods()
                .Where(x => x.Name == "IntersectBy")
                .Single(x => x.GetParameters().Length == 3)
                .MakeGenericMethod(typeof(TListItem), typeof(TListItemProp));
            var listItemParameter = Expression.Parameter(listItemType, "li");
            var listItemPropertyEx = Expression.MakeMemberAccess(listItemParameter, property);
            var listItemPropertyLamba = Expression.Lambda(listItemPropertyEx, listItemParameter);
            var countProperty = typeof(IEnumerable<>).MakeGenericType(typeof(TListItemProp)).GetProperty("Count");
            var intersectByExpression = Expression.Call(intersectByMethodInfo, memberAccess, valueList, listItemPropertyLamba);
            var intersectedCount = Expression.MakeMemberAccess(intersectByExpression, countProperty);
            Expression? finalExpression = null;
            switch (filter.FilterType)
            {
                case ValueListFilterType.Exact:
                    {
                        var valueCount = Expression.Constant(filter.ValueList.Count);
                        finalExpression = Expression.Equal(intersectedCount, valueCount);
                        break;
                    }
                case ValueListFilterType.Including:
                    {
                        var valueCount = Expression.Constant(filter.ValueList.Count);
                        finalExpression = Expression.GreaterThanOrEqual(intersectedCount, valueCount);
                        break;
                    }
                case ValueListFilterType.Excluding:
                    {
                        var valueCount = Expression.Constant(0);
                        finalExpression = Expression.Equal(intersectedCount, valueCount);
                        break;
                    }
                case ValueListFilterType.Any:
                    {
                        var valueCount = Expression.Constant(0);
                        finalExpression = Expression.GreaterThan(intersectedCount, valueCount);
                        break;
                    }
                default:
                    break;
            }
            return finalExpression;
        }

        public static Expression? GetSingleValueFilteringExpression<TProfile, TProperty>(SingleValueFilter<TProperty> filter, string propertyName) where TProfile : class, IProfile
        {
            var propertyType = typeof(TProperty);
            var property = typeof(TProfile).GetProperty(propertyName);
            if (filter == null || property == null || property.PropertyType != propertyType) return null;
            var value = Expression.Constant(filter.Value, propertyType);
            var parameter = Expression.Parameter(typeof(TProperty), "profile");
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            return filter.FilterType switch
            {
                SingleValueFilterType.Equals => Expression.Equal(value, memberAccess),
                SingleValueFilterType.DoesNotEqual => Expression.NotEqual(value, memberAccess),
                _ => null,
            };
        }

        public static IQueryable<TProfile> SortWith<TProfile>(this IQueryable<TProfile> query, Sort? config) where TProfile : class, IProfile
        {
            if (config == null) return query;
            string command = config.SortDirection == SortDirection.Desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TProfile);
            var property = type.GetProperty(config.SortBy);
            var parameter = Expression.Parameter(type, "p");
            if (property == null) return query;
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, [type, property.PropertyType], query.Expression, Expression.Quote(orderByExpression));
            return query.Provider.CreateQuery<TProfile>(resultExpression);
        }
    }
}
