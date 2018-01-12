using System;
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
            if (entity.Version == 0)
            {
                entity.SubmittedById = userId;
                entity.SubmittedDate = now;
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
                    var vote = await DbContext.TeamVotes.SingleOrDefaultAsync(x => x.TeamId == id && x.UserId == userId);
                    detail.MyVote = vote?.Value ?? 0;
                }
            }
            return await base.SingleGetTransform(single, id);
        }

        protected override IQueryable<Team> Filter(IQueryable<Team> entities)
        {
            if (!User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
            {
                entities = entities.Where(x => !x.Deleted);
            }
            return base.Filter(entities);
        }

        protected override bool CanPost(int? id)
        {
            return User.GetId() != null && User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) || OwnsTeam(id);
        }

        protected override bool CanDelete(int? id)
        {
            return CanPost(id);
        }

        protected override IQueryable<Team> OrderSearchResults(IQueryable<Team> results)
        {
            return results.OrderByDescending(x => x.EditedDate);
        }

        protected bool OwnsTeam(int? id)
        {
            if (!id.HasValue)
            {
                return true;
            }
            var userId = User.GetId();
            return DbContext.Teams.Any(x => x.Id == id && x.SubmittedById == userId);
        }

        protected override async Task<IQueryable<Team>> PerformSearch(IQueryable<Team> results, TeamSearchModel model)
        {
            results = SearchDeleted(results, model.Deleted);
            results = SearchReported(results, model.Reported);
            results = SearchStage(results, model.StageId);
            results = SearchTerm(results, model.Term);
            results = SearchSubmitter(results, model.SubmittedBy);
            results = SearchLead(results, model.LeaderId);
            results = SearchGlobal(results, model.Global);
            results = SearchFreeToPlay(results, model.FreeToPlay, model.LeaderId);
            results = SearchBox(results, model.MyBox);
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

        private IQueryable<Team> SearchReported(IQueryable<Team> results, bool modelReported)
        {
            if (User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) && modelReported)
            {
                results = results.Where(x => x.TeamReports.Any(y => !y.AcknowledgedDate.HasValue));
            }
            return results;
        }

        private IQueryable<Team> SearchTerm(IQueryable<Team> teams, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                teams = teams.Where(x => x.Name.Contains(term));
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

        private IQueryable<Team> SearchLead(IQueryable<Team> teams, int? leaderId)
        {
            if (leaderId.HasValue)
            {
                teams = teams.Where(x => x.TeamUnits.Any(y => y.UnitId == leaderId && y.Position < 2));
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

        private IQueryable<Team> SearchBox(IQueryable<Team> teams, bool myBox)
        {
            if (myBox)
            {
                throw new System.NotImplementedException();
            }
            return teams;
        }

        private IQueryable<Team> SearchFreeToPlay(IQueryable<Team> results, bool freeToPlay, int? leaderId)
        {
            if (freeToPlay)
            {
                results = results.Where(x => x.TeamUnits.All(y => y.Sub || y.Position == 0 || y.UnitId == leaderId || !EnumerableHelper.PayToPlay.Any(z => y.Unit.Flags.HasFlag(z))));
            }
            return results;
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
                UserId = userId
            };
            var value = model.Up.HasValue ? (model.Up ?? true) ? 1 : -1 : 0;
            if (model.Up.HasValue)
            {
                if (!exists)
                {
                    DbContext.TeamVotes.Add(vote);
                }
                vote.Value = (short)value;
            }
            else if (exists)
            {
                DbContext.TeamVotes.Remove(vote);
            }
            await DbContext.SaveChangesAsync();
            var returnValue = await DbContext.TeamVotes.Where(x => x.TeamId == teamId).Select(x => x.Value).DefaultIfEmpty((short)0).SumAsync(x => x);
            return Ok(returnValue);
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

            var videos = model.Videos.Select(x => new TeamVideo
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
