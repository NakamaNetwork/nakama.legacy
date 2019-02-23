using System.Linq;
using System.Security.Claims;
using NakamaNetwork.Entities.Models;
using TreasureGuide.Common.Models;

namespace TreasureGuide.Web.Services.QueryService
{
    public interface ISearchableDatabaseQueryService<TEntity, TSearchModel> : IDatabaseQueryService<TEntity>
        where TEntity : class
        where TSearchModel : SearchModel
    {
        IQueryable<TEntity> Search(TSearchModel model, IQueryable<TEntity> results = null, ClaimsPrincipal user = null);
    }

    public abstract class SearchableDatabaseQueryService<TEntity, TSearchModel> : DatabaseQueryService<TEntity>, ISearchableDatabaseQueryService<TEntity, TSearchModel>
        where TEntity : class
        where TSearchModel : SearchModel
    {
        public SearchableDatabaseQueryService(NakamaNetworkContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<TEntity> Search(TSearchModel model, IQueryable<TEntity> results = null, ClaimsPrincipal user = null)
        {
            if (results == null)
            {
                results = DbContext.Set<TEntity>().AsQueryable();
            }
            results = PerformSearch(model, results, user);
            results = PerformSort(model, results);
            results = PerformPagination(model, results);
            return results;
        }

        protected virtual IQueryable<TEntity> PerformPagination(TSearchModel model, IQueryable<TEntity> results)
        {
            return results.Skip(model.Page * model.PageSize).Take(model.PageSize);
        }

        protected abstract IQueryable<TEntity> PerformSort(TSearchModel model, IQueryable<TEntity> results);

        protected abstract IQueryable<TEntity> PerformSearch(TSearchModel model, IQueryable<TEntity> results, ClaimsPrincipal user);
    }
}
