using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TreasureGuide.Common.Models.TeamModels;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Services.SearchService.Teams
{
    public abstract class TeamSearchService : ISearchService<Team, TeamSearchModel>
    {
        public abstract Task<IQueryable<Team>> Search(IQueryable<Team> results, TeamSearchModel model, ClaimsPrincipal user = null);
        public abstract Task RebuildIndex(IQueryable<Team> input, bool clearOld = false);
    }
}
