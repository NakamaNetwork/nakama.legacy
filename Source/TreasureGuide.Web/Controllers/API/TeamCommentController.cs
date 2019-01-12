using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Common.Constants;
using TreasureGuide.Common.Helpers;
using TreasureGuide.Common.Models.TeamModels;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/teamcomment")]
    public class TeamCommentController : SearchableApiController<int, TeamComment, int?, TeamCommentStubModel, TeamCommentDetailModel, TeamCommentEditorModel, TeamCommentSearchModel>
    {
        private const int COMMENT_TIMEOUT = 30;

        public TeamCommentController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        [HttpGet]
        [ActionName("LoadMore")]
        [Route("[action]")]
        public async Task<IActionResult> LoadMore(int id, int current)
        {
            var found = DbContext.TeamComments.Where(x => x.ParentId == id);
            found = Filter(found);
            var results = await found.OrderBy(x => x.Id).Skip(current).Take(10)
                .ProjectTo<TeamCommentDetailModel>(AutoMapper.ConfigurationProvider).ToListAsync();

            return Ok(results);
        }

        protected override async Task<object> PerformPost(TeamCommentEditorModel model, int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request, Request.Path, COMMENT_TIMEOUT))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            return await base.PerformPost(model, id);
        }

        protected override async Task<TeamComment> PostProcess(TeamComment entity)
        {
            var userId = User.GetId();
            var now = DateTimeOffset.Now;
            if (entity.Version == 0 || entity.SubmittedById == null)
            {
                entity.SubmittedById = userId;
                entity.SubmittedDate = now;
                entity.TeamCommentVotes = new List<TeamCommentVote>
                {
                    new TeamCommentVote
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

        protected override IQueryable<TeamComment> Filter(IQueryable<TeamComment> entities)
        {
            if (!User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
            {
                entities = entities.Where(x => !x.Deleted);
            }
            return base.Filter(entities);
        }

        protected override IQueryable<TModel> Project<TModel>(IQueryable<TeamComment> entities)
        {
            if (typeof(TModel) == typeof(TeamCommentStubModel))
            {
                return entities.ProjectTo<TModel>(AutoMapper.ConfigurationProvider,
                    new
                    {
                        userId = User.GetId(),
                        canEdit = (User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
                    });
            }
            return base.Project<TModel>(entities);
        }

        protected override async Task<object> Remove(TeamComment single)
        {
            if (single != null)
            {
                single.Deleted = true;
                await SaveChangesAsync();
            }
            return 1;
        }

        protected override bool CanPost(int? id)
        {
            var userId = User.GetId();
            return !String.IsNullOrWhiteSpace(userId) && (User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) || OwnsComment(id, userId));
        }

        protected override bool CanDelete(int? id)
        {
            return CanPost(id);
        }

        protected bool OwnsComment(int? id, string userId)
        {
            if (!id.HasValue || id == 0)
            {
                return true;
            }
            return DbContext.TeamComments.Any(x => x.Id == id && x.SubmittedById == userId);
        }

        protected override async Task<IQueryable<TeamComment>> PerformSearch(IQueryable<TeamComment> results, TeamCommentSearchModel model)
        {
            if (model.TeamId != 0 || !User.IsInRole(RoleConstants.Administrator))
            {
                results = results.Where(x => x.TeamId == model.TeamId && x.ParentId == null);
            }
            if (model.Deleted)
            {
                results = results.Where(x => x.Deleted);
            }
            if (model.Reported)
            {
                results = results.Where(x => x.Reported);
            }
            return results;
        }

        protected override IQueryable<TeamComment> OrderSearchResults(IQueryable<TeamComment> results, TeamCommentSearchModel model)
        {
            switch (model.SortBy ?? "")
            {
                case SearchConstants.SortDate:
                    return results.OrderBy(x => x.SubmittedDate, !model.SortDesc);
                default:
                    return results.OrderBy(x => x.TeamCommentVotes.Select(y => (int)y.Value).DefaultIfEmpty(0).Sum(), !model.SortDesc);
            }
        }

        [HttpPost]
        [Authorize]
        [ActionName("Vote")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Vote([FromBody] TeamCommentVoteModel model, int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            var commentId = id ?? model.TeamCommentId;
            var userId = User.GetId();
            var vote = await DbContext.TeamCommentVotes.SingleOrDefaultAsync(x => x.TeamCommentId == model.TeamCommentId && x.UserId == userId);
            var exists = vote != null;
            vote = vote ?? new TeamCommentVote
            {
                TeamCommentId = commentId,
                UserId = userId,
                SubmittedDate = DateTimeOffset.Now
            };
            var value = model.Up.HasValue ? (model.Up ?? true) ? 1 : -1 : 0;
            if (!exists)
            {
                DbContext.TeamCommentVotes.Add(vote);
            }
            vote.Value = (short)value;
            await DbContext.SaveChangesAsync();
            var returnValue = await DbContext.TeamCommentVotes.Where(x => x.TeamCommentId == commentId).Select(x => x.Value).DefaultIfEmpty((short)0).SumAsync(x => x);
            return Ok(returnValue);
        }

        [HttpPost]
        [Authorize]
        [ActionName("Report")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Report([FromBody] TeamCommentReportModel model, int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            var teamCommentId = id ?? model.TeamCommentId;

            var team = DbContext.TeamComments.SingleOrDefault(x => x.Id == teamCommentId);
            if (team != null)
            {
                team.Reported = true;
                await DbContext.SaveChangesAsync();
            }
            return Ok(teamCommentId);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Administrator + "," + RoleConstants.Moderator)]
        [ActionName("Acknowledge")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Acknowledge([FromBody] TeamCommentReportModel model, int? id = null)
        {
            var teamCommentId = id ?? model.TeamCommentId;

            var team = DbContext.TeamComments.SingleOrDefault(x => x.Id == teamCommentId);
            if (team != null)
            {
                team.Reported = false;
                await DbContext.SaveChangesAsync();
            }
            return Ok(teamCommentId);
        }
    }
}
