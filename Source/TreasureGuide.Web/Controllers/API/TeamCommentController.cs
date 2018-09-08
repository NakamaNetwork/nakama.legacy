using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        public TeamCommentController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
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
            results = results.Where(x => x.TeamId == model.TeamId);
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
    }
}
