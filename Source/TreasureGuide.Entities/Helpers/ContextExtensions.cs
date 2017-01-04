using System.Data.Entity;

namespace TreasureGuide.Entities.Helpers
{
    public static class ContextExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
