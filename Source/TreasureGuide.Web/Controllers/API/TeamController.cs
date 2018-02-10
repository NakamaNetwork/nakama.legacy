using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models.TeamModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/team")]
    public class TeamController : SearchableApiController<int, Team, int?, TeamStubModel, TeamDetailModel, TeamEditorModel, TeamSearchModel>
    {
        public TeamController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override async Task<Team> PostProcess(Team entity)
        {
            var userId = User.GetId();
            var now = DateTimeOffset.Now;
            if (entity.Version == 0 || entity.SubmittedById == null)
            {
                entity.SubmittedById = userId;
                entity.SubmittedDate = now;
                entity.TeamVotes = new List<TeamVote>
                {
                    new TeamVote
                    {
                        SubmittedDate = now,
                        UserId = userId,
                        Value = 1
                    }
                };
            }
            entity.EditedById = userId;
            entity.EditedDate = now;
            entity.Version = entity.Version + 1;
            return await base.PostProcess(entity);
        }

        protected override async Task<TModel> SingleGetTransform<TModel>(TModel single, int? id = null)
        {
            var detail = single as TeamDetailModel;
            if (detail != null)
            {
                if (!User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
                {
                    detail.TeamVideos = detail.TeamVideos.Where(x => !x.Deleted);
                }
                var userId = User.GetId();
                if (userId != null)
                {
                    var user = DbContext.UserProfiles.Where(x => x.Id == userId);
                    detail.MyVote = await user.SelectMany(x => x.TeamVotes.Where(y => y.TeamId == id).Select(y => y.Value)).SingleOrDefaultAsync();
                    detail.MyBookmark = await user.SelectMany(x => x.BookmarkedTeams.Where(y => y.Id == id).Select(y => true)).SingleOrDefaultAsync();
                }
            }
            return await base.SingleGetTransform(single, id);
        }

        protected override IQueryable<Team> Filter(IQueryable<Team> entities)
        {
            var userId = User.GetId();
            if (!User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
            {
                entities = entities.Where(x => !x.Deleted && (!x.Draft || x.SubmittedById == userId));
            }
            return base.Filter(entities);
        }

        protected override bool CanPost(int? id)
        {
            var userId = User.GetId();
            return !String.IsNullOrWhiteSpace(userId) && (User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) || OwnsTeam(id, userId));
        }

        protected override bool CanDelete(int? id)
        {
            return CanPost(id);
        }

        protected override IQueryable<Team> OrderSearchResults(IQueryable<Team> results, TeamSearchModel model)
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
                    return results.OrderBy(x => x.TeamUnits.Where(y => y.Position == 1 && !y.Sub).Select(y => y.Unit.Name).DefaultIfEmpty("").FirstOrDefault());
                case SearchConstants.SortScore:
                    return results.OrderBy(x => x.TeamVotes.Select(y => (int)y.Value).DefaultIfEmpty(0).Sum(), !model.SortDesc);
                case SearchConstants.SortDate:
                    return results.OrderBy(x => x.EditedDate, !model.SortDesc);
                case SearchConstants.SortUser:
                    return results.OrderBy(x => x.SubmittingUser.UserName, model.SortDesc);
                default:
                    return results.OrderBy(x => x.EditedDate, true);
            }
        }

        protected bool OwnsTeam(int? id, string userId)
        {
            if (!id.HasValue || id == 0)
            {
                return true;
            }
            return DbContext.Teams.Any(x => x.Id == id && x.SubmittedById == userId);
        }

        protected override async Task<IQueryable<Team>> PerformSearch(IQueryable<Team> results, TeamSearchModel model)
        {
            results = SearchDeleted(results, model.Deleted);
            results = SearchDrafts(results, model.Draft);
            results = SearchReported(results, model.Reported);
            results = SearchBookmarks(results, model.Bookmark);
            results = SearchStage(results, model.StageId);
            results = SearchTerm(results, model.Term);
            results = SearchSubmitter(results, model.SubmittedBy);
            results = SearchLead(results, model.LeaderId, model.NoHelp);
            results = SearchGlobal(results, model.Global);
            results = SearchFreeToPlay(results, model.FreeToPlay, model.LeaderId);
            results = SearchTypes(results, model.Types);
            results = SearchClasses(results, model.Classes);
            results = SearchFreeToPlay(results, model.FreeToPlay, model.LeaderId);
            results = SearchBox(results, model.Box);
            return results;
        }

        private IQueryable<Team> SearchDeleted(IQueryable<Team> results, bool modelDeleted)
        {
            if (!User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
            {
                results = results.Where(x => !x.Deleted);
            }
            else
            {
                results = results.Where(x => x.Deleted == modelDeleted);
            }
            return results;
        }

        private IQueryable<Team> SearchDrafts(IQueryable<Team> results, bool modelDraft)
        {
            if (!User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
            {
                results = results.Where(x => !x.Draft);
            }
            else
            {
                results = results.Where(x => x.Draft == modelDraft);
            }
            return results;
        }

        private IQueryable<Team> SearchReported(IQueryable<Team> results, bool modelReported)
        {
            if (User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) && modelReported)
            {
                results = results.Where(x => x.TeamReports.Any(y => !y.AcknowledgedDate.HasValue));
            }
            return results;
        }

        private IQueryable<Team> SearchBookmarks(IQueryable<Team> results, bool bookmarked)
        {
            if (bookmarked)
            {
                var userId = User.GetId();
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

        private IQueryable<Team> SearchStage(IQueryable<Team> teams, int? stageId)
        {
            if (stageId.HasValue)
            {
                teams = teams.Where(x => x.StageId == stageId);
            }
            return teams;
        }

        private IQueryable<Team> SearchLead(IQueryable<Team> teams, int? leaderId, bool noHelper)
        {
            if (leaderId.HasValue)
            {
                teams = teams.Where(x => x.TeamUnitSummaries.Any(y => y.UnitId == leaderId && (noHelper ? y.Position == 1 : y.Position < 2) && !y.Sub));
            }
            return teams;
        }

        private IQueryable<Team> SearchGlobal(IQueryable<Team> teams, bool global)
        {
            if (global)
            {
                teams = teams.Where(x => x.TeamUnitSummaries.All(y => y.Sub || y.Flags.HasFlag(UnitFlag.Global)));
            }
            return teams;
        }

        private IQueryable<Team> SearchBox(IQueryable<Team> teams, int? boxId)
        {
            if (boxId.HasValue)
            {
                teams = teams.Where(x => x.TeamUnits.All(y => y.Sub || y.Unit.Boxes.Any(z => z.Id == boxId && !z.Blacklist)));
            }
            return teams;
        }

        private IQueryable<Team> SearchFreeToPlay(IQueryable<Team> results, FreeToPlayStatus status, int? leaderId)
        {
            if (status != FreeToPlayStatus.None)
            {
                results = results.Where(x => x.TeamUnitSummaries.All(y =>
                    y.Sub || // Ignore subs
                    y.Position == 0 || // Ignore friends
                    y.UnitId == leaderId || // Ignore searched captain
                    ( // Ignore leaders if only searching crew
                        (status == FreeToPlayStatus.Crew && y.Position < 2) || !EnumerableHelper.PayToPlay.Any(z => y.Flags.HasFlag(z))
                    )
                ));
            }
            return results;
        }

        private IQueryable<Team> SearchTypes(IQueryable<Team> results, UnitType modelTypes)
        {
            if (modelTypes != UnitType.Unknown)
            {
                results = results.Where(x => x.TeamUnitSummaries.All(y => y.Sub || y.Type == UnitType.Unknown || (y.Type & modelTypes) != 0));
            }
            return results;
        }

        private IQueryable<Team> SearchClasses(IQueryable<Team> results, UnitClass modelClasses)
        {
            if (modelClasses != UnitClass.Unknown)
            {
                results = results.Where(x => x.TeamUnitSummaries.All(y => y.Sub || y.Class == UnitClass.Unknown || (y.Class & modelClasses) != 0));
            }
            return results;
        }

        [HttpGet]
        [ActionName("Similar")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> Similar(int? id)
        {
            var similar = DbContext.SimilarTeamsId(id).OrderBy(x => x.Matches).Take(10);
            return await TrimDownSimilar(similar);
        }

        [HttpGet]
        [ActionName("Similar")]
        [Route("[action]")]
        public async Task<IActionResult> Similar(int? teamId, int? stageId, int? unit1, int? unit2, int? unit3, int? unit4, int? unit5, int? unit6)
        {
            var similar = DbContext.SimilarTeams(teamId, stageId, unit1, unit2, unit3, unit4, unit5, unit6).OrderByDescending(x => x.Matches).Take(10);
            return await TrimDownSimilar(similar);
        }

        private async Task<IActionResult> TrimDownSimilar(IQueryable<SimilarTeams_Result> similar)
        {
            var teamIds = await similar.Select(x => x.TeamId).ToListAsync();
            var teams = await DbContext.Teams.Join(teamIds, x => x.Id, y => y, (x, y) => x).ProjectTo<TeamStubModel>(AutoMapper.ConfigurationProvider).ToListAsync();
            return Ok(teams);
        }

        [HttpGet]
        [ActionName("Trending")]
        [Route("[action]")]
        public async Task<IActionResult> Trending()
        {
            var threshold = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(1.5));
            var top = await DbContext.Teams
                .Where(x => !x.Deleted && !x.Draft)
                .Select(x => new
                {
                    team = x,
                    trendScore = x.TeamVotes
                        .Where(y => y.SubmittedDate > threshold)
                        .Select(y => (int)y.Value)
                        .DefaultIfEmpty()
                        .Sum()
                })
                .Where(x => x.trendScore > 0)
                .OrderByDescending(x => x.trendScore)
                .Take(5)
                .Select(x => x.team)
                .ProjectTo<TeamStubModel>(AutoMapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(top);
        }

        [HttpGet]
        [ActionName("Latest")]
        [Route("[action]")]
        public async Task<IActionResult> Latest()
        {
            var threshold = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(3));
            var top = await DbContext.Teams
                .Where(x => !x.Deleted && !x.Draft && x.SubmittedDate > threshold)
                .OrderByDescending(x => x.SubmittedDate)
                .Take(5)
                .ProjectTo<TeamStubModel>(AutoMapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(top);
        }

        [HttpPost]
        [Authorize]
        [ActionName("Vote")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Vote([FromBody] TeamVoteModel model, int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode((int)HttpStatusCode.Conflict, ThrottleService.Message);
            }
            var teamId = id ?? model.TeamId;
            var userId = User.GetId();
            var vote = await DbContext.TeamVotes.SingleOrDefaultAsync(x => x.TeamId == model.TeamId && x.UserId == userId);
            var exists = vote != null;
            vote = vote ?? new TeamVote
            {
                TeamId = teamId,
                UserId = userId,
                SubmittedDate = DateTimeOffset.Now
            };
            var value = model.Up.HasValue ? (model.Up ?? true) ? 1 : -1 : 0;
            if (!exists)
            {
                DbContext.TeamVotes.Add(vote);
            }
            vote.Value = (short)value;
            await DbContext.SaveChangesAsync();
            var returnValue = await DbContext.TeamVotes.Where(x => x.TeamId == teamId).Select(x => x.Value).DefaultIfEmpty((short)0).SumAsync(x => x);
            return Ok(returnValue);
        }


        [HttpPost]
        [Authorize]
        [ActionName("Bookmark")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Bookmark(int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode((int)HttpStatusCode.Conflict, ThrottleService.Message);
            }
            var team = await DbContext.Teams.SingleOrDefaultAsync(x => x.Id == id);
            if (team == null)
            {
                return BadRequest("Could not find team.");
            }
            var userId = User.GetId();
            var profile = await DbContext.UserProfiles.SingleOrDefaultAsync(x => x.Id == userId);
            if (profile == null)
            {
                return Unauthorized();
            }
            var existed = team.BookmarkedUsers.Any(x => x.Id == userId);
            if (existed)
            {
                team.BookmarkedUsers.Remove(profile);
            }
            else
            {
                team.BookmarkedUsers.Add(profile);
            }
            await DbContext.SaveChangesAsync();
            return Ok(!existed);
        }

        [HttpPost]
        [Authorize]
        [ActionName("Report")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Report([FromBody] TeamReportModel model, int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode((int)HttpStatusCode.Conflict, ThrottleService.Message);
            }
            var teamId = id ?? model.TeamId;

            DbContext.TeamReports.Add(new TeamReport
            {
                TeamId = teamId,
                Reason = model.Reason
            });
            await DbContext.SaveChangesAsync();
            return Ok(teamId);
        }


        [HttpGet]
        [Authorize(Roles = RoleConstants.Administrator + "," + RoleConstants.Moderator)]
        [ActionName("Reports")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Reports(int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode((int)HttpStatusCode.Conflict, ThrottleService.Message);
            }
            var reports = await DbContext
                .TeamReports
                .Where(x => x.TeamId == id)
                .OrderBy(x => x.AcknowledgedDate)
                .ThenBy(x => x.TeamId)
                .ProjectTo<TeamReportStubModel>(AutoMapper.ConfigurationProvider)
                .ToArrayAsync();
            return Ok(reports);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Administrator + "," + RoleConstants.Moderator)]
        [ActionName("AcknowledgeReport")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> AcknowledgeReport(int? id = null)
        {
            var team = await DbContext.TeamReports.SingleOrDefaultAsync(x => x.Id == id);
            if (team == null)
            {
                return BadRequest("Could not find report.");
            }
            team.AcknowledgedDate = DateTimeOffset.Now;
            await DbContext.SaveChangesAsync();
            return Ok(id);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Contributor)]
        [ActionName("Video")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Video([FromBody] TeamVideoModel model, int? id = null)
        {
            var videoId = id ?? model.Id;
            TeamVideo video = null;
            if (videoId.HasValue)
            {
                video = await DbContext.TeamVideos.SingleOrDefaultAsync(x => x.Id == videoId);
            }
            var exists = video != null;
            if (exists && !(await CanEditVideo(video.Id)))
            {
                return Unauthorized();
            }
            video = video ?? new TeamVideo();
            video.UserId = User.GetId();
            video.Deleted = model.Deleted;
            video.SubmittedDate = DateTimeOffset.Now;
            video.VideoLink = model.VideoLink;
            video.TeamId = model.TeamId;
            if (!exists)
            {
                DbContext.TeamVideos.Add(video);
            }
            await DbContext.SaveChangesAsync();
            return Ok(video.Id);
        }

        private async Task<bool> CanEditVideo(int? videoId)
        {
            var userId = User.GetId();
            return userId != null && User.IsInAnyRole(RoleConstants.Moderator, RoleConstants.Administrator) ||
                   await DbContext.TeamVideos.AnyAsync(x => x.Id == videoId && x.UserId == userId);
        }

        public const string ImportId = "112cf4e4-cb26-4293-afa2-e663785fd276";

        [HttpPost]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ActionName("Import")]
        [Route("[action]")]
        public async Task<IActionResult> Import([FromBody] TeamImportModel model)
        {
            var now = DateTimeOffset.Now;
            var team = AutoMapper.Map<Team>(model.Team);
            team.SubmittedDate = now;
            team.SubmittedById = ImportId;
            team.EditedDate = now;
            team.EditedById = ImportId;
            team.Draft = true;
            DbContext.Teams.Add(team);

            if (!String.IsNullOrWhiteSpace(model.Credit.Credit))
            {
                var credit = new TeamCredit
                {
                    Team = team,
                    Credit = model.Credit.Credit,
                    Type = model.Credit.Type
                };
                DbContext.TeamCredits.Add(credit);
            }

            var videos = model.Videos.Where(x => !String.IsNullOrWhiteSpace(x.VideoLink)).Select(x => new TeamVideo
            {
                Team = team,
                SubmittedDate = now,
                UserId = ImportId,
                VideoLink = x.VideoLink
            }).ToList();
            DbContext.TeamVideos.AddRange(videos);

            await DbContext.SaveChangesAsync();

            return Ok(team.Id);
        }
    }
}
