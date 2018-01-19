using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TreasureGuide.Entities.Helpers
{
    public static class EnumerableHelper
    {
        public static readonly IEnumerable<UnitFlag> PayToPlay = new[]
        {
            UnitFlag.RareRecruitExclusive,
            UnitFlag.RareRecruitLimited
        };

        public static IQueryable<TEntity> FindId<TNullable, TEntity>(this IQueryable<TEntity> queryable, TNullable id = default(TNullable))
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
