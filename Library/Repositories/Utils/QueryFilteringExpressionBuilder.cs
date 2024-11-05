using Library.Exceptions;
using Library.Models.Enums;
using Library.Models.QueryConditions;
using System.Linq.Expressions;
using System.Numerics;

namespace Library.Repositories.Utils
{
    public class QueryFilteringExpressionBuilder<T> where T : class
    {
        private ParameterExpression _parameter;
        private Expression _combinedExpression;

        private QueryFilteringExpressionBuilder()
        {
            _combinedExpression = Expression.Constant(true);
        }

        public QueryFilteringExpressionBuilder(string parameterName) : this()
        {
            _parameter = Expression.Parameter(typeof(T), parameterName);
        }

        public QueryFilteringExpressionBuilder<T> ApplyStringFiltering(StringFilter? filter, string propertyName)
        {
            if (filter == null) return this;
            Type strType = typeof(string);
            Type profileType = typeof(T);
            var property = profileType.GetProperty(propertyName);
            if (property == null || property.PropertyType != strType) throw new InvalidClassMemberException(propertyName, profileType.Name);
            var inputParam = Expression.Constant(filter.Input, strType);
            var memberAccess = Expression.MakeMemberAccess(_parameter, property);
            var containsMethodInfo = strType.GetMethod("Contains", [strType]);
            var startsWithMethodInfo = strType.GetMethod("StartsWith", [strType]);
            var endsWithMethodInfo = strType.GetMethod("EndsWith", [strType]);
            _combinedExpression = filter.FilterType switch
            {
                StringValueFilterType.Equal => Expression.AndAlso(_combinedExpression, Expression.Equal(memberAccess, inputParam)),
                StringValueFilterType.NotEqual => Expression.AndAlso(_combinedExpression, Expression.NotEqual(memberAccess, inputParam)),
                StringValueFilterType.Contains => Expression.AndAlso(_combinedExpression, Expression.Call(memberAccess, containsMethodInfo, inputParam)),
                StringValueFilterType.DoesNotContain => Expression.AndAlso(_combinedExpression, Expression.Not(Expression.Call(memberAccess, containsMethodInfo, inputParam))),
                StringValueFilterType.StartsWith => Expression.AndAlso(_combinedExpression, Expression.Call(memberAccess, startsWithMethodInfo, inputParam)),
                StringValueFilterType.EndsWith => Expression.AndAlso(_combinedExpression, Expression.Call(memberAccess, endsWithMethodInfo, inputParam)),
                _ => throw new InvalidEnumMemberException($"{filter.FilterType}", typeof(StringValueFilterType).Name),
            };
            return this;
        }

        public QueryFilteringExpressionBuilder<T> ApplyDateTimeFiltering(DateTimeFilter? filter, string timePropertyName)
        {
            if (filter == null) return this;
            var property = typeof(T).GetProperty(timePropertyName);
            var dateTimeType = typeof(DateTime);
            if (property == null || property.PropertyType != dateTimeType) throw new InvalidClassMemberException(timePropertyName, typeof(T).Name);
            var inputParam = Expression.Constant(filter.DateTime, dateTimeType);
            var memberAccess = Expression.MakeMemberAccess(_parameter, property);
            _combinedExpression = filter.FilterType switch
            {
                DateTimeFilterType.Before => Expression.AndAlso(_combinedExpression, Expression.LessThan(memberAccess, inputParam)),
                DateTimeFilterType.AtOrBefore => Expression.AndAlso(_combinedExpression, Expression.LessThanOrEqual(memberAccess, inputParam)),
                DateTimeFilterType.Exact => Expression.AndAlso(_combinedExpression, Expression.Equal(memberAccess, inputParam)),
                DateTimeFilterType.AtOrAfter => Expression.AndAlso(_combinedExpression, Expression.GreaterThanOrEqual(memberAccess, inputParam)),
                DateTimeFilterType.After => Expression.AndAlso(_combinedExpression, Expression.GreaterThan(memberAccess, inputParam)),
                _ => throw new InvalidEnumMemberException($"{filter.FilterType}", typeof(DateTimeFilterType).Name),
            };
            return this;
        }

        public QueryFilteringExpressionBuilder<T> ApplyNumericFiltering<TNumber>(NumericFilter<TNumber>? filter, string numberPropertyName)
            where TNumber : INumber<TNumber>
        {
            if (filter == null) return this;
            var property = typeof(T).GetProperty(numberPropertyName);
            var numberType = typeof(TNumber);
            if (property == null || property.PropertyType != numberType) throw new InvalidClassMemberException(numberPropertyName, typeof(T).Name);
            var inputParam = Expression.Constant(filter.Input, numberType);
            var memberAccess = Expression.MakeMemberAccess(_parameter, property);
            _combinedExpression = filter.FilterType switch
            {
                NumericFilterType.Equal => Expression.AndAlso(_combinedExpression, Expression.Equal(memberAccess, inputParam)),
                NumericFilterType.NotEqual => Expression.AndAlso(_combinedExpression, Expression.NotEqual(memberAccess, inputParam)),
                NumericFilterType.Less => Expression.AndAlso(_combinedExpression, Expression.LessThan(memberAccess, inputParam)),
                NumericFilterType.LessOrEqual => Expression.AndAlso(_combinedExpression, Expression.LessThanOrEqual(memberAccess, inputParam)),
                NumericFilterType.Greater => Expression.AndAlso(_combinedExpression, Expression.GreaterThan(memberAccess, inputParam)),
                NumericFilterType.GreaterOrEqual => Expression.AndAlso(_combinedExpression, Expression.GreaterThanOrEqual(memberAccess, inputParam)),
                _ => throw new InvalidEnumMemberException($"{filter.FilterType}", typeof(NumericFilterType).Name),
            };
            return this;
        }

        public QueryFilteringExpressionBuilder<T> ApplyValueListFiltering<TListItemProp>(ValueListFilter<TListItemProp> filter, string valuePropertyName)
        {
            if (filter == null) return this;
            var propertyType = typeof(TListItemProp);
            var property = typeof(T).GetProperty(valuePropertyName);
            if (property == null || property.PropertyType != propertyType) throw new InvalidClassMemberException(valuePropertyName, typeof(T).Name);
            var valueList = Expression.Constant(filter.ValueList, typeof(IEnumerable<TListItemProp>));
            var memberAccess = Expression.MakeMemberAccess(_parameter, property);
            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .Where(x => x.Name == "Contains")
                .Single(x => x.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TListItemProp));
            switch (filter.FilterType)
            {
                case ValueListFilterType.Exact:
                    {
                        if (filter.ValueList.Count > 1)
                        {
                            _combinedExpression = Expression.AndAlso(_combinedExpression, Expression.Constant(false));
                        }
                        else
                        {
                            _combinedExpression = Expression.AndAlso(_combinedExpression, Expression.Call(containsMethod, valueList, memberAccess));
                        }
                        break;
                    }
                case ValueListFilterType.Any:
                case ValueListFilterType.Including:
                    {
                        _combinedExpression = Expression.AndAlso(_combinedExpression, Expression.Call(containsMethod, valueList, memberAccess));
                        break;
                    }
                case ValueListFilterType.Excluding:
                    {
                        _combinedExpression = Expression.AndAlso(_combinedExpression, Expression.Not(Expression.Call(containsMethod, valueList, memberAccess)));
                        break;
                    }
                default:
                    throw new InvalidEnumMemberException($"{filter.FilterType}", typeof(ValueListFilterType).Name);
            }
            return this;
        }

        public QueryFilteringExpressionBuilder<T> ApplyValueListOnMemberListFiltering<TMemberListItem, TListItemProp>(ValueListFilter<TListItemProp>? filter, string listPropertyName, string listItemPropertyName)
        {
            if (filter == null) return this;
            var listType = typeof(ICollection<TMemberListItem>);
            var property = typeof(T).GetProperty(listPropertyName);
            var listItemProperty = typeof(TMemberListItem).GetProperty(listItemPropertyName);
            if (property == null || property.PropertyType != listType)
                throw new InvalidClassMemberException(listPropertyName, typeof(T).Name);
            if (listItemProperty == null || listItemProperty.PropertyType != typeof(TListItemProp))
                throw new InvalidClassMemberException(listItemPropertyName, typeof(TMemberListItem).Name);
            var valueList = Expression.Constant(filter.ValueList, typeof(ICollection<TListItemProp>));
            var memberAccess = Expression.Property(_parameter, listPropertyName);
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
            switch (filter.FilterType)
            {
                case ValueListFilterType.Including:
                    {
                        var valueCount = Expression.Constant(filter.ValueList.Count);
                        _combinedExpression = Expression.AndAlso(_combinedExpression, Expression.GreaterThanOrEqual(intersectedCount, valueCount));
                        break;
                    }
                case ValueListFilterType.Exact:
                    {
                        var valueCount = Expression.Constant(filter.ValueList.Count);
                        _combinedExpression = Expression.AndAlso(_combinedExpression, Expression.Equal(intersectedCount, valueCount));
                        break;
                    }
                case ValueListFilterType.Excluding:
                    {
                        var valueCount = Expression.Constant(0);
                        _combinedExpression = Expression.AndAlso(_combinedExpression, Expression.Equal(intersectedCount, valueCount));
                        break;
                    }
                case ValueListFilterType.Any:
                    {
                        var valueCount = Expression.Constant(0);
                        _combinedExpression = Expression.AndAlso(_combinedExpression, Expression.GreaterThan(intersectedCount, valueCount));
                        break;
                    }
                default:
                    throw new InvalidEnumMemberException($"{filter.FilterType}", typeof(ValueListFilterType).Name);
            }
            return this;
        }

        public QueryFilteringExpressionBuilder<T> ApplySingleValueFiltering<TProperty>(SingleValueFilter<TProperty> filter, string propertyName)
        {
            if (filter == null) return this;
            var propertyType = typeof(TProperty);
            var property = typeof(T).GetProperty(propertyName);
            if (property == null || property.PropertyType != propertyType) throw new InvalidClassMemberException(propertyName, typeof(T).Name);
            var value = Expression.Constant(filter.Value, propertyType);
            var memberAccess = Expression.MakeMemberAccess(_parameter, property);
            _combinedExpression = filter.FilterType switch
            {
                SingleValueFilterType.Equals => Expression.AndAlso(_combinedExpression, Expression.Equal(value, memberAccess)),
                SingleValueFilterType.DoesNotEqual => Expression.AndAlso(_combinedExpression, Expression.NotEqual(value, memberAccess)),
                _ => throw new InvalidEnumMemberException($"{filter.FilterType}", typeof(ValueListFilterType).Name),
            };
            return this;
        }

        public Expression<Func<T, bool>> BuildLambdaExpression()
        {
            Expression<Func<T, bool>> finalLambda = Expression.Lambda<Func<T, bool>>(_combinedExpression, _parameter);
            return finalLambda;
        }
    }
}
