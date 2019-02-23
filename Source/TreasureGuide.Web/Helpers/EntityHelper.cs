using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NakamaNetwork.Entities.Models;

namespace TreasureGuide.Web.Helpers
{
    public static class EntityHelper
    {
        private const int SqlServerViolationOfUniqueIndex = 2601;
        private const int SqlServerViolationOfUniqueConstraint = 2627;

        public static async Task SaveChangesSafe(this NakamaNetworkContext entities)
        {
            try
            {
                await entities.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                entities.Database.RollbackTransaction();
                throw dbEx;
            }
        }

        public static void RemoveWhere<TEntity>(this ICollection<TEntity> entities, Func<TEntity, bool> predicate)
            where TEntity : class
        {
            var found = entities.Where(predicate);
            foreach (var item in found)
            {
                entities.Remove(item);
            }
        }

        public static void RemoveWhere<TEntity>(this DbSet<TEntity> entities, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var found = entities.Where(predicate);
            entities.RemoveRange(found);
        }
    }
}
