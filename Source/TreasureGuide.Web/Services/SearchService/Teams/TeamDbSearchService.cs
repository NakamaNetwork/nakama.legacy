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
    public class TeamDbSearchService : TeamSearchService
    {
        private readonly TreasureEntities _entities;

        public TeamDbSearchService(TreasureEntities entities)
        {
            _entities = entities;
        }

        public override async Task<IQueryable<Team>> Search(IQueryable<Team> results, TeamSearchModel model, ClaimsPrincipal user = null)
        {
            results = SearchDeleted(results, model.Deleted, user);
            results = SearchDrafts(results, model.Draft, user);
            results = SearchReported(results, model.Reported, user);
            results = SearchEventShips(results, model.ExcludeEventShips);
            results = SearchBookmarks(results, model.Bookmark, user);
            results = SearchStage(results, model.StageId, model.InvasionId);
            results = SearchTerm(results, model.Term);
            results = SearchSubmitter(results, model.SubmittedBy);
            results = SearchLead(results, model.LeaderId, model.NoHelp);
            results = SearchGlobal(results, model.Global);
            results = SearchFreeToPlay(results, model.FreeToPlay, model.LeaderId);
            results = SearchTypes(results, model.Types);
            results = SearchClasses(results, model.Classes);
            results = SearchFreeToPlay(results, model.FreeToPlay, model.LeaderId);
            results = SearchBox(results, model.BoxId);
            return results;
        }

        private IQueryable<Team> SearchDeleted(IQueryable<Team> results, bool modelDeleted, ClaimsPrincipal user)
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

        private IQueryable<Team> SearchDrafts(IQueryable<Team> results, bool modelDraft, ClaimsPrincipal user)
        {
            results = results.Where(x => x.Draft == modelDraft);
            if (modelDraft && !(user?.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) ?? false))
            {
                var userId = user.GetId();
                results = results.Where(x => x.SubmittedById == userId);
            }
            return results;
        }

        private IQueryable<Team> SearchReported(IQueryable<Team> results, bool modelReported, ClaimsPrincipal user)
        {
            if (modelReported && (user?.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) ?? false))
            {
                results = results.Where(x => x.TeamReports.Any(y => !y.AcknowledgedDate.HasValue));
            }
            return results;
        }

        private IQueryable<Team> SearchEventShips(IQueryable<Team> results, bool excludeEventShips)
        {
            if (excludeEventShips)
            {
                results = results.Where(x => !x.Ship.EventShip);
            }
            return results;
        }

        private IQueryable<Team> SearchBookmarks(IQueryable<Team> results, bool bookmarked, ClaimsPrincipal user)
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

        private IQueryable<Team> SearchTerm(IQueryable<Team> teams, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                var terms = term.SplitSearchTerms();
                teams = teams.Where(x => terms.All(t => x.Name.Contains(t) || (x.Stage != null && x.Stage.Name.Contains(t))));
            }
            return teams;
        }

        private IQueryable<Team> SearchSubmitter(IQueryable<Team> teams, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                teams = teams.Where(x => x.SubmittedById == term || x.SubmittingUser.UserName.Contains(term));
            }
            return teams;
        }

        private IQueryable<Team> SearchStage(IQueryable<Team> teams, int? stageId, int? invasionId)
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

        private IQueryable<Team> SearchLead(IQueryable<Team> teams, int? leaderId, bool noHelper)
        {
            if (leaderId.HasValue)
            {
                teams = teams.Where(x => x.TeamUnits.Any(y => y.UnitId == leaderId && (noHelper ? y.Position == 1 : y.Position < 2) && !y.Sub));
            }
            return teams;
        }

        private IQueryable<Team> SearchGlobal(IQueryable<Team> teams, bool global)
        {
            if (global)
            {
                teams = teams.Where(x => x.TeamUnits.All(y => y.Sub || y.Unit.Flags.HasFlag(UnitFlag.Global)));
            }
            return teams;
        }

        private IQueryable<Team> SearchBox(IQueryable<Team> teams, int? boxId)
        {
            if (boxId.HasValue)
            {
                teams = teams.Where(x => x.TeamUnits.All(y => y.Sub || y.Position == 0 || y.Unit.BoxUnits.Any(z => z.BoxId == boxId)));
            }
            return teams;
        }

        private IQueryable<Team> SearchFreeToPlay(IQueryable<Team> results, FreeToPlayStatus status, int? leaderId)
        {
            if (status != FreeToPlayStatus.None)
            {
                results = results.Where(x => x.TeamUnits.All(y =>
                    y.Sub || // Ignore subs
                    y.Position == 0 || // Ignore friends
                    y.UnitId == leaderId || // Ignore searched captain
                    ( // Ignore leaders if only searching crew
                        (status == FreeToPlayStatus.Crew && y.Position < 2) || !EnumerableHelper.PayToPlay.Any(z => y.Unit.Flags.HasFlag(z))
                    )
                ));
            }
            return results;
        }

        private IQueryable<Team> SearchTypes(IQueryable<Team> results, UnitType modelTypes)
        {
            if (modelTypes != UnitType.Unknown)
            {
                results = results.Where(x => x.TeamUnits.All(y => y.Sub || y.Unit.Type == UnitType.Unknown || (y.Unit.Type & modelTypes) != 0));
            }
            return results;
        }

        private IQueryable<Team> SearchClasses(IQueryable<Team> results, UnitClass modelClasses)
        {
            if (modelClasses != UnitClass.Unknown)
            {
                results = results.Where(x => x.TeamUnits.All(y => y.Sub || y.Unit.Class == UnitClass.Unknown || (y.Unit.Class & modelClasses) != 0));
            }
            return results;
        }

        public override async Task RebuildIndex(IQueryable<Team> input, bool clearAll = false)
        {
            // Nothing to do.
        }
    }
}
