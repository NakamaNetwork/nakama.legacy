﻿using System;
using System.Linq;
using System.Linq.Expressions;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Helpers
{
    public static class EnumerableExtensions
    {
        public static IQueryable<TEntity> FindId<TKey, TEntity>(this IQueryable<TEntity> queryable, TKey? id = null)
            where TKey : struct
            where TEntity : IIdItem<TKey>
        {
            var arg = Expression.Parameter(typeof(TEntity), "i");
            var predicate =
                Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(
                        Expression.Property(arg, "Id"),
                        Expression.Constant(id)),
                    arg);
            return queryable.Where(predicate);
        }
    }
}