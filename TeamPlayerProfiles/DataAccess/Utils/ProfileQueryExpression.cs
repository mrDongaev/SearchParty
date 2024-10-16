using Common.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Numerics;
using static Common.Models.ConditionalQuery;
using static Common.Models.ConditionalQuery.Filter;

namespace DataAccess.Utils
{
    public static class ProfileQueryExpression
    {
        public static Expression GetStringFilteringExpression<T>(this Expression expr, StringFilter? filter, string propertyName, ParameterExpression parameter)
            where T : class
        {
            if (filter == null) return expr;
            Type strType = typeof(string);
            Type profileType = typeof(T);
            var property = profileType.GetProperty(propertyName);
            if (property == null || property.PropertyType != strType) throw new ArgumentException($"{propertyName} property does not exist on {profileType.Name} type or the property type doesn't match the class member");
            var inputParam = Expression.Constant(filter.Input, strType);
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            var containsMethodInfo = strType.GetMethod("Contains", [strType]);
            var startsWithMethodInfo = strType.GetMethod("StartsWith", [strType]);
            var endsWithMethodInfo = strType.GetMethod("EndsWith", [strType]);
            return filter.FilterType switch
            {
                StringValueFilterType.Equal => Expression.AndAlso(expr, Expression.Equal(memberAccess, inputParam)),
                StringValueFilterType.NotEqual => Expression.AndAlso(expr, Expression.NotEqual(memberAccess, inputParam)),
                StringValueFilterType.Contains => Expression.AndAlso(expr, Expression.Call(memberAccess, containsMethodInfo, inputParam)),
                StringValueFilterType.DoesNotContain => Expression.AndAlso(expr, Expression.Not(Expression.Call(memberAccess, containsMethodInfo, inputParam))),
                StringValueFilterType.StartsWith => Expression.AndAlso(expr, Expression.Call(memberAccess, startsWithMethodInfo, inputParam)),
                StringValueFilterType.EndsWith => Expression.AndAlso(expr, Expression.Call(memberAccess, endsWithMethodInfo, inputParam)),
                _ => throw new InvalidEnumArgumentException($"{filter.FilterType} does not exist on {typeof(StringValueFilterType).Name} type"),
            };
        }

        public static Expression GetDateTimeFilteringExpression<T>(this Expression expr, TimeFilter? filter, string timePropertyName, ParameterExpression parameter)
            where T : class
        {
            if (filter == null) return expr;
            var property = typeof(T).GetProperty(timePropertyName);
            var dateTimeType = typeof(DateTime);
            if (property == null || property.PropertyType != dateTimeType) throw new ArgumentException($"{timePropertyName} property does not exist on {typeof(T).Name} type or the property type doesn't match the class member");
            var inputParam = Expression.Constant(filter.DateTime, dateTimeType);
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            return filter.FilterType switch
            {
                DateTimeFilter.Before => Expression.AndAlso(expr, Expression.LessThan(memberAccess, inputParam)),
                DateTimeFilter.AtOrBefore => Expression.AndAlso(expr, Expression.LessThanOrEqual(memberAccess, inputParam)),
                DateTimeFilter.Exact => Expression.AndAlso(expr, Expression.Equal(memberAccess, inputParam)),
                DateTimeFilter.AtOrAfter => Expression.AndAlso(expr, Expression.GreaterThanOrEqual(memberAccess, inputParam)),
                DateTimeFilter.After => Expression.AndAlso(expr, Expression.GreaterThan(memberAccess, inputParam)),
                _ => throw new InvalidEnumArgumentException($"{filter.FilterType} does not exist on {typeof(DateTimeFilter).Name} type"),
            };
        }

        public static Expression GetNumericFilteringExpression<T, TNumber>(this Expression expr, NumericFilter<TNumber>? filter, string numberPropertyName, ParameterExpression parameter)
            where T : class
            where TNumber : INumber<TNumber>
        {
            if (filter == null) return expr;
            var property = typeof(T).GetProperty(numberPropertyName);
            var numberType = typeof(TNumber);
            if (property == null || property.PropertyType != numberType) throw new ArgumentException($"{numberPropertyName} property does not exist on {typeof(T).Name} type or the property type doesn't match the class member");
            var inputParam = Expression.Constant(filter.Input, numberType);
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            return filter.FilterType switch
            {
                NumericFilterType.Equal => Expression.AndAlso(expr, Expression.Equal(memberAccess, inputParam)),
                NumericFilterType.NotEqual => Expression.AndAlso(expr, Expression.NotEqual(memberAccess, inputParam)),
                NumericFilterType.Less => Expression.AndAlso(expr, Expression.LessThan(memberAccess, inputParam)),
                NumericFilterType.LessOrEqual => Expression.AndAlso(expr, Expression.LessThanOrEqual(memberAccess, inputParam)),
                NumericFilterType.Greater => Expression.AndAlso(expr, Expression.GreaterThan(memberAccess, inputParam)),
                NumericFilterType.GreaterOrEqual => Expression.AndAlso(expr, Expression.GreaterThanOrEqual(memberAccess, inputParam)),
                _ => throw new InvalidEnumArgumentException($"{filter.FilterType} does not exist on {typeof(NumericFilterType).Name} type"),
            };
        }

        public static Expression GetValueListFilteringExpression<T, TListItemProp>(this Expression expr, ValueFilter<TListItemProp> filter, string valuePropertyName, ParameterExpression parameter)
            where T : class
        {
            if (filter == null) return expr;
            var propertyType = typeof(TListItemProp);
            var property = typeof(T).GetProperty(valuePropertyName);
            if (property == null || property.PropertyType != propertyType) throw new ArgumentException($"{valuePropertyName} property does not exist on {typeof(T).Name} type or the property type doesn't match the class member");
            var valueList = Expression.Constant(filter.ValueList, typeof(IEnumerable<TListItemProp>));
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
                        if (filter.ValueList.Count > 1)
                        {
                            finalExpression = Expression.AndAlso(expr, Expression.Constant(false));
                        }
                        else
                        {
                            finalExpression = Expression.AndAlso(expr, Expression.Call(containsMethod, valueList, memberAccess));
                        }
                        break;
                    }
                case ValueListFilterType.Any:
                case ValueListFilterType.Including:
                    {
                        finalExpression = Expression.AndAlso(expr, Expression.Call(containsMethod, valueList, memberAccess));
                        break;
                    }
                case ValueListFilterType.Excluding:
                    {
                        finalExpression = Expression.AndAlso(expr, Expression.Not(Expression.Call(containsMethod, valueList, memberAccess)));
                        break;
                    }
                default:
                    throw new InvalidEnumArgumentException($"{filter.FilterType} does not exist on {typeof(ValueListFilterType).Name} type");
            }
            return finalExpression;
        }

        public static Expression GetValueListOnMemberListFilteringExpression<T, TMemberListItem, TListItemProp>(
            this Expression expr, ValueFilter<TListItemProp>? filter,
            string listPropertyName,
            string listItemPropertyName,
            ParameterExpression parameter) where T : class
        {
            if (filter == null) return expr;
            var listType = typeof(ICollection<TMemberListItem>);
            var property = typeof(T).GetProperty(listPropertyName);
            var listItemProperty = typeof(TMemberListItem).GetProperty(listItemPropertyName);
            if (property == null || property.PropertyType != listType) 
                throw new ArgumentException($"{listPropertyName} property does not exist on {typeof(T).Name} type or the property type doesn't match the class member");
            if (listItemProperty == null || listItemProperty.PropertyType != typeof(TListItemProp))
                throw new ArgumentException($"{listItemPropertyName} property does not exist on {typeof(TMemberListItem).Name} type or the property type doesn't match the class member");
            var valueList = Expression.Constant(filter.ValueList, typeof(ICollection<TListItemProp>));
            var memberAccess = Expression.Property(parameter, listPropertyName);
            var asQueryable = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "AsQueryable" && m.IsGenericMethodDefinition);
            var asQueryableListItemProp = asQueryable.MakeGenericMethod(typeof(TListItemProp));
            var asQueryableListItem = asQueryable.MakeGenericMethod(typeof(TMemberListItem));
            var select = typeof(Queryable)
                .GetMethods()
                .Where(m => m.Name == "Select" && m.IsGenericMethodDefinition)
                .Single(m =>
                {
                    var parameters = m.GetParameters();
                    if (parameters.Length != 2)
                        return false;
                    var delegateType = parameters[1].ParameterType.GetGenericArguments()[0];
                    return delegateType.IsGenericType && delegateType.GetGenericTypeDefinition() == typeof(Func<,>);
                }).MakeGenericMethod(typeof(TMemberListItem), typeof(TListItemProp));
            var intersect = typeof(Queryable)
                .GetMethods()
                .Where(x => x.Name == "Intersect")
                .Single(x => x.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TListItemProp));
            var count = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Count" && m.GetParameters().Length == 1)
                .MakeGenericMethod(typeof(TListItemProp));
            var listItemParameter = Expression.Parameter(typeof(TMemberListItem), "li");
            var listItemPropertyEx = Expression.Property(listItemParameter, listItemPropertyName);
            var listItemPropertyLambda = Expression.Lambda(listItemPropertyEx, listItemParameter);
            var asQueryableExpression = Expression.Call(asQueryableListItem, memberAccess);
            var valueListAsQueryable = Expression.Call(asQueryableListItemProp, valueList);
            var selectExpression = Expression.Call(select, asQueryableExpression, listItemPropertyLambda);
            var intersectExpression = Expression.Call(intersect, selectExpression, valueListAsQueryable);
            var intersectedCount = Expression.Call(count, intersectExpression);
            Expression? finalExpression = null;
            switch (filter.FilterType)
            {
                case ValueListFilterType.Including:
                    {
                        var valueCount = Expression.Constant(filter.ValueList.Count);
                        finalExpression = Expression.AndAlso(expr, Expression.GreaterThanOrEqual(intersectedCount, valueCount));
                        break;
                    }
                case ValueListFilterType.Exact:
                    {
                        var valueCount = Expression.Constant(filter.ValueList.Count);
                        finalExpression = Expression.AndAlso(expr, Expression.Equal(intersectedCount, valueCount));
                        break;
                    }
                case ValueListFilterType.Excluding:
                    {
                        var valueCount = Expression.Constant(0);
                        finalExpression = Expression.AndAlso(expr, Expression.Equal(intersectedCount, valueCount));
                        break;
                    }
                case ValueListFilterType.Any:
                    {
                        var valueCount = Expression.Constant(0);
                        finalExpression = Expression.AndAlso(expr, Expression.GreaterThan(intersectedCount, valueCount));
                        break;
                    }
                default:
                    throw new InvalidEnumArgumentException($"{filter.FilterType} does not exist on {typeof(ValueListFilterType).Name} type");
            }
            return finalExpression;
        }

        public static Expression GetSingleValueFilteringExpression<T, TProperty>(this Expression expr, SingleValueFilter<TProperty> filter, string propertyName, ParameterExpression parameter)
            where T : class
        {
            if (filter == null) return expr;
            var propertyType = typeof(TProperty);
            var property = typeof(T).GetProperty(propertyName);
            if (property == null || property.PropertyType != propertyType) throw new ArgumentException($"{propertyName} property does not exist on {typeof(T).Name} type or the property type doesn't match the class member");
            var value = Expression.Constant(filter.Value, propertyType);
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            return filter.FilterType switch
            {
                SingleValueFilterType.Equals => Expression.AndAlso(expr, Expression.Equal(value, memberAccess)),
                SingleValueFilterType.DoesNotEqual => Expression.AndAlso(expr, Expression.NotEqual(value, memberAccess)),
                _ => throw new InvalidEnumArgumentException($"{filter.FilterType} does not exist on {typeof(ValueListFilterType).Name} type"),
            };
        }

        public static IQueryable<T> SortWith<T>(this IQueryable<T> query, Sort? config) where T : class
        {
            if (config == null) return query;
            string command = config.SortDirection switch
            {
                SortDirection.Asc => "OrderBy",
                SortDirection.Desc => "OrderByDescending",
                _ => throw new InvalidEnumArgumentException($"{config.SortDirection} does not exist on {typeof(SortDirection).Name} type"),
            };
            var type = typeof(T);
            var property = type.GetProperty(config.SortBy);
            var parameter = Expression.Parameter(type, "entity");
            if (property == null) return query;
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, [type, property.PropertyType], query.Expression, Expression.Quote(orderByExpression));
            return query.Provider.CreateQuery<T>(resultExpression);
        }

    }
}
