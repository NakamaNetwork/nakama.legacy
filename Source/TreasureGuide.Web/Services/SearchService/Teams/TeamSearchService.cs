using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TreasureGuide.Common.Models.TeamModels;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Services.SearchService.Teams
{
    public abstract class TeamSearchService : ISearchService<int, Team, TeamSearchModel>
    {
        public abstract Task<IQueryable<Team>> Search(IQueryable<Team> input, TeamSearchModel model, ClaimsPrincipal user = null);
        public abstract Task RebuildFullIndex();
        public abstract Task RebuildIndex(Team model);
    }
}
