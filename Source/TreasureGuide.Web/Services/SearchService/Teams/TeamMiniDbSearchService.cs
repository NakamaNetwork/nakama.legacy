using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TreasureGuide.Common.Constants;
using TreasureGuide.Common.Helpers;
using TreasureGuide.Common.Models.TeamModels;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;

namespace TreasureGuide.Web.Services.SearchService.Teams
{
    public class TeamMiniDbSearchService : TeamSearchService
    {
        private TreasureEntities _entities;

        public TeamMiniDbSearchService(TreasureEntities entities)
        {
            _entities = entities;
        }

        private class TeamMini : Team
        {
            public bool EventShip { get; set; }
            public bool HasReport { get; set; }
            public string StageName { get; set; }
            public string SubmittingUserName { get; set; }
            public bool Global { get; set; }
            public bool F2P { get; set; }
            public bool F2PC { get; set; }
            public int Unit0 { get; set; }
            public int Unit1 { get; set; }
            public int Unit2 { get; set; }
            public int Unit3 { get; set; }
            public int Unit4 { get; set; }
            public int Unit5 { get; set; }
            public UnitType TypeFlags { get; set; }
            public UnitClass ClassFlags { get; set; }
        }

        public override async Task<IQueryable<Team>> Search(IQueryable<Team> results, TeamSearchModel model, ClaimsPrincipal user = null)
        {
            var minis = Enumerable.Empty<TeamMini>().AsQueryable();
            minis = SearchDeleted(minis, model.Deleted, user);
            minis = SearchDrafts(minis, model.Draft, user);
            minis = SearchReported(minis, model.Reported, user);
            minis = SearchEventShips(minis, model.ExcludeEventShips);
            minis = SearchBookmarks(minis, model.Bookmark, user);
            minis = SearchStage(minis, model.StageId, model.InvasionId);
            minis = SearchTerm(minis, model.Term);
            minis = SearchSubmitter(minis, model.SubmittedBy);
            minis = SearchLead(minis, model.LeaderId, model.NoHelp);
            minis = SearchGlobal(minis, model.Global);
            minis = SearchFreeToPlay(minis, model.FreeToPlay, model.LeaderId);
            minis = SearchTypes(minis, model.Types);
            minis = SearchClasses(minis, model.Classes);
            minis = SearchFreeToPlay(minis, model.FreeToPlay, model.LeaderId);
            minis = SearchBox(minis, model.BoxId);

            results = minis.Join(results, x => x.Id, y => y.Id, (x, y) => y);
            return results;
        }

        private IQueryable<TeamMini> SearchDeleted(IQueryable<TeamMini> results, bool modelDeleted, ClaimsPrincipal user)
        {
            if (user?.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) ?? false)
            {
                results = results.Where(x => x.Deleted == modelDeleted);
            }
            else
            {
                results = results.Where(x => !x.Deleted);
            }
            return results;
        }

        private IQueryable<TeamMini> SearchDrafts(IQueryable<TeamMini> results, bool modelDraft, ClaimsPrincipal user)
        {
            results = results.Where(x => x.Draft == modelDraft);
            if (modelDraft && !(user?.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) ?? false))
            {
                var userId = user.GetId();
                results = results.Where(x => x.SubmittedById == userId);
            }
            return results;
        }

        private IQueryable<TeamMini> SearchReported(IQueryable<TeamMini> results, bool modelReported, ClaimsPrincipal user)
        {
            if (modelReported && (user?.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) ?? false))
            {
                results = results.Where(x => x.HasReport);
            }
            return results;
        }

        private IQueryable<TeamMini> SearchEventShips(IQueryable<TeamMini> results, bool excludeEventShips)
        {
            if (excludeEventShips)
            {
                results = results.Where(x => !x.EventShip);
            }
            return results;
        }

        private IQueryable<TeamMini> SearchBookmarks(IQueryable<TeamMini> results, bool bookmarked, ClaimsPrincipal user)
        {
            if (bookmarked)
            {
                var userId = user?.GetId();
                if (!String.IsNullOrWhiteSpace(userId))
                {
                    results = results.Where(x => x.BookmarkedUsers.Any(y => y.Id == userId));
                }
            }
            return results;
        }

        private IQueryable<TeamMini> SearchTerm(IQueryable<TeamMini> teams, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                var terms = term.SplitSearchTerms();
                teams = teams.Where(x => terms.All(t => x.Name.Contains(t) || (x.StageName.Contains(t))));
            }
            return teams;
        }

        private IQueryable<TeamMini> SearchSubmitter(IQueryable<TeamMini> teams, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                teams = teams.Where(x => x.SubmittedById == term || x.SubmittingUserName.Contains(term));
            }
            return teams;
        }

        private IQueryable<TeamMini> SearchStage(IQueryable<TeamMini> teams, int? stageId, int? invasionId)
        {
            if (stageId.HasValue)
            {
                teams = teams.Where(x => x.StageId == stageId || x.InvasionId == stageId);
            }
            if (invasionId.HasValue)
            {
                teams = teams.Where(x => x.StageId == invasionId || x.InvasionId == invasionId);
            }
            return teams;
        }

        private IQueryable<TeamMini> SearchLead(IQueryable<TeamMini> teams, int? leaderId, bool noHelper)
        {
            if (leaderId.HasValue)
            {
                teams = teams.Where(x => x.Unit1 == leaderId || (!noHelper && x.Unit0 == leaderId));
            }
            return teams;
        }

        private IQueryable<TeamMini> SearchGlobal(IQueryable<TeamMini> teams, bool global)
        {
            if (global)
            {
                teams = teams.Where(x => x.Global);
            }
            return teams;
        }

        private IQueryable<TeamMini> SearchBox(IQueryable<TeamMini> teams, int? boxId)
        {
            if (boxId.HasValue)
            {
                teams = teams.Where(x => x.TeamUnits.All(y => y.Sub || y.Position == 0 || y.Unit.BoxUnits.Any(z => z.BoxId == boxId)));
            }
            return teams;
        }

        private IQueryable<TeamMini> SearchFreeToPlay(IQueryable<TeamMini> results, FreeToPlayStatus status, int? leaderId)
        {
            if (status != FreeToPlayStatus.None)
            {
                results = results.Where(x => status == FreeToPlayStatus.Crew ? x.F2PC : (x.F2P || x.Unit1 == leaderId));
            }
            return results;
        }

        private IQueryable<TeamMini> SearchTypes(IQueryable<TeamMini> results, UnitType modelTypes)
        {
            if (modelTypes != UnitType.Unknown)
            {
                results = results.Where(x => (x.TypeFlags & modelTypes) != 0);
            }
            return results;
        }

        private IQueryable<TeamMini> SearchClasses(IQueryable<TeamMini> results, UnitClass modelClasses)
        {
            if (modelClasses != UnitClass.Unknown)
            {
                results = results.Where(x => (x.ClassFlags & modelClasses) != 0);
            }
            return results;
        }

        public override async Task RebuildIndex(IQueryable<Team> input, bool clearAll = false)
        {
            // Nothing to do.
        }
    }
}
