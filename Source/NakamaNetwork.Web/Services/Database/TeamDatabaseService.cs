using System.Linq;
using NakamaNetwork.Entities.Models;
using TreasureGuide.Common.Models.TeamModels;

namespace NakamaNetwork.Web.Services.Database
{
    public class TeamDatabaseService : IdDatabaseService<int, Team>, ISearchableDatabaseService<Team, TeamSearchModel>
    {
        public TeamDatabaseService(NakamaNetworkContext context) : base(context)
        {
        }

        public IQueryable<Team> Search(TeamSearchModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
