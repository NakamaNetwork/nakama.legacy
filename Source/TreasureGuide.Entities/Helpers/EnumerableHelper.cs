using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> queryable, Expression<Func<T, TKey>> selector, bool desc)
        {
            if (desc)
            {
                return queryable.OrderByDescending(selector);
            }
            return queryable.OrderBy(selector);
        }

        public static IOrderedQueryable<T> ThenBy<T, TKey>(this IOrderedQueryable<T> queryable, Expression<Func<T, TKey>> selector, bool desc)
        {
            if (desc)
            {
                return queryable.ThenByDescending(selector);
            }
            return queryable.ThenBy(selector);
        }

        private static readonly Regex AlphaNumericRegex = new Regex(@"/[^\w\d]/");
        private static readonly char[] Splitters = { ' ' };

        public static IEnumerable<string> SplitSearchTerms(this string term)
        {
            return AlphaNumericRegex.Replace(term, " ").Split(Splitters, StringSplitOptions.RemoveEmptyEntries);
        }

        public static async Task LoopedAddSave<T>(this DbContext context, IEnumerable<T> entities, int pageSize = 100)
            where T : class
        {
            var count = 0;
            var total = entities.Count();
            Debug.WriteLine($"Performing bulk save on {total} items");
            IEnumerable<T> slice;
            while ((slice = entities.Skip(count * pageSize).Take(pageSize)).Any())
            {
                Debug.WriteLine($"   {(count * pageSize)}/{total}");
                context.Set<T>().AddRange(slice);
                await context.SaveChangesAsync();
                count++;
            }
            Debug.WriteLine($"   {total}/{total}");
            Debug.WriteLine("Bulk save complete.");
        }
    }
}
