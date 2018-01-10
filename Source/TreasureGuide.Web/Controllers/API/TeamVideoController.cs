using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models.TeamModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/video")]
    public class TeamVideoController : EntityApiController<int, TeamVideo, int?, TeamVideoModel, TeamVideoModel, TeamVideoModel>
    {
        public TeamVideoController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override bool CanGet(int? id)
        {
            return false;
        }

        protected override bool CanPost(int? id)
        {
            return User.GetId() != null && User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) || OwnsVideo(id);
        }

        protected override bool CanDelete(int? id)
        {
            return CanPost(id);
        }

        protected bool OwnsVideo(int? id)
        {
            if (!id.HasValue)
            {
                return true;
            }
            var userId = User.GetId();
            return DbContext.TeamVideos.Any(x => x.Id == id && x.UserId == userId);
        }

    }
}
