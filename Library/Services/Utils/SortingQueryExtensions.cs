using Library.Exceptions;
using Library.Models.Enums;
using Library.Models.QueryConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Utils
{
    public static class SortingQueryExtensions
    {
        public static IQueryable<T> SortWith<T>(this IQueryable<T> query, SortCondition? config) where T : class
        {
            if (config == null) return query;
            string command = config.SortDirection switch
            {
                SortDirection.Asc => "OrderBy",
                SortDirection.Desc => "OrderByDescending",
                _ => throw new InvalidEnumMemberException($"{config.SortDirection}", typeof(SortDirection).Name),
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
