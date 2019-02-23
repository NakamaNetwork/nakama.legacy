using System;
using System.Linq;
using System.Security.Claims;
using NakamaNetwork.Entities.EnumTypes;
using NakamaNetwork.Entities.Helpers;
using NakamaNetwork.Entities.Models;
using TreasureGuide.Common.Constants;
using TreasureGuide.Common.Helpers;
using TreasureGuide.Common.Models.TeamModels;

namespace TreasureGuide.Web.Services.QueryService
{
    public interface ITeamDatabaseQueryService : ISearchableDatabaseQueryService<Team, TeamSearchModel>
    {
        IQueryable<Team> SearchWiki(TeamSearchModel model);
        IQueryable<Team> GetTrending();
        IQueryable<Team> GetLatest();
    }

    public class TeamDatabaseQueryService : SearchableDatabaseQueryService<Team, TeamSearchModel>, ITeamDatabaseQueryService
    {
        public TeamDatabaseQueryService(NakamaNetworkContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Team> PerformSearch(TeamSearchModel model, IQueryable<Team> results, ClaimsPrincipal user)
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
                    results = results.Where(x => x.TeamBookmarks.Any(y => y.UserId == userId));
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
                teams = teams.Where(x => x.SubmittedById == term || x.SubmittedBy.UserName.Contains(term));
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

        protected override IQueryable<Team> PerformSort(TeamSearchModel model, IQueryable<Team> results)
        {
            switch (model.SortBy ?? "")
            {
                case SearchConstants.SortId:
                    return results.OrderBy(x => x.Id, model.SortDesc);
                case SearchConstants.SortName:
                    return results.OrderBy(x => x.Name, model.SortDesc);
                case SearchConstants.SortStage:
                    return results.OrderBy(x => x.Stage != null ? x.Stage.Name : "", model.SortDesc);
                case SearchConstants.SortLeader:
                    return results.OrderBy(x => x.TeamUnits.Where(y => y.Position == 1 && !y.Sub).Select(y => y.Unit.Name).DefaultIfEmpty("").FirstOrDefault(), model.SortDesc);
                case SearchConstants.SortScore:
                    return results.OrderBy(x => x.TeamVotes.DefaultIfEmpty().Sum(y => y.Value), !model.SortDesc);
                case SearchConstants.SortDate:
                    return results.OrderBy(x => x.SubmittedDate, !model.SortDesc);
                case SearchConstants.SortUser:
                    return results.OrderBy(x => x.SubmittedBy.UserName, model.SortDesc);
                default:
                    return results.OrderBy(x => x.Id, true);
            }
        }

        public IQueryable<Team> SearchWiki(TeamSearchModel model)
        {
            return Search(model);
        }

        public IQueryable<Team> GetTrending()
        {
            var recent = DateTimeOffset.Now.AddDays(-1.5);
            return DbContext.TeamVotes
                .Where(x => x.SubmittedDate > recent)
                .GroupBy(x => x.Team)
                .Select(x => new
                {
                    Team = x.Key,
                    Score = x.DefaultIfEmpty().Sum(y => y.Value)
                })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Team)
                .Take(10);
        }

        public IQueryable<Team> GetLatest()
        {
            return DbContext.Teams.OrderByDescending(x => x.Id).Take(5);
        }
    }
}
