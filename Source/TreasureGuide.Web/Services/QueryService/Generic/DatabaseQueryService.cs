using NakamaNetwork.Entities.Models;
using System.Linq;
using System.Threading.Tasks;
using TreasureGuide.Web.Helpers;

namespace TreasureGuide.Web.Services.QueryService
{
    public interface IDatabaseQueryService<TEntity>
        where TEntity : class
    {
        Task<TEntity> FindAsync(params object[] ids);
        TEntity CreateOrUpdate(TEntity entity);
        TEntity Delete(TEntity entity);
        Task SaveChangesAsync();
    }

    public class DatabaseQueryService<TEntity> : IDatabaseQueryService<TEntity>
        where TEntity : class
    {
        protected readonly NakamaNetworkContext DbContext;

        public DatabaseQueryService(NakamaNetworkContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual TEntity CreateOrUpdate(TEntity entity)
        {
            var found = DbContext.Attach(entity);
            return found.Entity;
        }

        public virtual TEntity Delete(TEntity entity)
        {
            var found = DbContext.Remove(entity);
            return found.Entity;
        }

        public virtual async Task<TEntity> FindAsync(params object[] ids)
        {
            return await DbContext.FindAsync<TEntity>(ids);
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesSafe();
        }
    }
}
