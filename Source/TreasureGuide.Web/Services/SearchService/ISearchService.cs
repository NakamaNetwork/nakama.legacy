using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TreasureGuide.Common.Models;

namespace TreasureGuide.Web.Services.SearchService
{
    public interface ISearchService<TEntity, TSearchModel>
        where TEntity : class
        where TSearchModel : SearchModel, new()
    {
        Task<IQueryable<TEntity>> Search(IQueryable<TEntity> results, TSearchModel model, ClaimsPrincipal user = null);
        Task RebuildIndex(IQueryable<TEntity> input, bool clearOld = false);
    }
}
