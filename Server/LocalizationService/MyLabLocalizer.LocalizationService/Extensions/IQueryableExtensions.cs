using System;
using System.Linq;
using System.Linq.Expressions;

namespace MyLabLocalizer.LocalizationService.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> query, Expression<Func<TSource, bool>> predicate, bool condition)
        {
            if (condition)
                return query.Where(predicate);
            else
                return query;
        }
    }
}
