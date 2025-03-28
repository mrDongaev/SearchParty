﻿using Library.Exceptions;
using Library.Models.Enums;
using Library.Models.QueryConditions;
using System.Linq.Expressions;

namespace Library.Repositories.Utils
{
    public class QuerySortingExpressionBuilder<T> where T : class
    {
        private IQueryable<T> _query;

        private Expression _combinedExpression;

        public bool AdditionalQuery { get; private set; }

        private QuerySortingExpressionBuilder(bool? additionalQuery = false)
        {
            AdditionalQuery = additionalQuery ?? false;
        }

        public QuerySortingExpressionBuilder(IQueryable<T> query, bool? additionalQuery = false) : this(additionalQuery)
        {
            _query = query;
            _combinedExpression = _query.Expression;
        }

        public QuerySortingExpressionBuilder<T> ApplySort(SortCondition? config)
        {
            if (config == null) return this;
            return ApplySort(config.SortBy, config.SortDirection);
        }

        public QuerySortingExpressionBuilder<T> ApplySort(IEnumerable<SortCondition>? configList)
        {
            if (configList == null) return this;
            foreach (var config in configList)
            {
                ApplySort(config.SortBy, config.SortDirection);
            }
            return this;
        }

        private QuerySortingExpressionBuilder<T> ApplySort(string parameterName, SortDirection sortDirection)
        {
            if (AdditionalQuery)
            {
                SortAdditional(parameterName, sortDirection);
            }
            else
            {
                SortFirst(parameterName, sortDirection);
                AdditionalQuery = true;
            }
            return this;
        }

        public IQueryable<T> GetSortedQuery()
        {
            return _query.Provider.CreateQuery<T>(_combinedExpression);
        }

        private void SortFirst(string parameterName, SortDirection sortDirection)
        {
            string command = sortDirection switch
            {
                SortDirection.Asc => "OrderBy",
                SortDirection.Desc => "OrderByDescending",
                _ => throw new InvalidEnumMemberException($"{sortDirection}", typeof(SortDirection).Name),
            };
            var type = typeof(T);
            var property = type.GetProperty(parameterName);
            var parameter = Expression.Parameter(type, "entity");
            if (property == null) return;
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, [type, property.PropertyType], _combinedExpression, Expression.Quote(orderByExpression));
            _combinedExpression = _query.Provider.CreateQuery<T>(resultExpression).Expression;
        }

        private void SortAdditional(string parameterName, SortDirection sortDirection)
        {
            string command = sortDirection switch
            {
                SortDirection.Asc => "ThenBy",
                SortDirection.Desc => "ThenByDescending",
                _ => throw new InvalidEnumMemberException($"{sortDirection}", typeof(SortDirection).Name),
            };
            var type = typeof(T);
            var property = type.GetProperty(parameterName);
            var parameter = Expression.Parameter(type, "entity");
            if (property == null) return;
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, [type, property.PropertyType], _combinedExpression, Expression.Quote(orderByExpression));
            _combinedExpression = _query.Provider.CreateQuery<T>(resultExpression).Expression;
        }
    }
}
