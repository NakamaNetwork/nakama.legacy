using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TreasureGuide.Common.Models;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Services.SearchService
{
    public interface ISearchService<TEntityKey, TEntity, TSearchModel>
        where TEntity : class, IIdItem<TEntityKey>
        where TSearchModel : SearchModel, new()
    {
        Task<IQueryable<TEntity>> Search(IQueryable<TEntity> input, TSearchModel model, ClaimsPrincipal user = null);
        Task RebuildFullIndex();
        Task RebuildIndex(TEntity model);
    }
}
