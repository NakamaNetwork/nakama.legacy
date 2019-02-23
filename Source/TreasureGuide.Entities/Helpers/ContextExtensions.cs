using Microsoft.EntityFrameworkCore;

namespace NakamaNetwork.Entities.Helpers
{
    public static class ContextExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
