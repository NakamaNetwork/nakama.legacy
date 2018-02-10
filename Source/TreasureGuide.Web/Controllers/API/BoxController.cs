using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models.BoxModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/box")]
    public class BoxController : SearchableApiController<int, Box, int?, BoxStubModel, BoxDetailModel, BoxEditorModel, BoxSearchModel>
    {
        public BoxController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override IQueryable<Box> Filter(IQueryable<Box> entities)
        {
            var userId = User.GetId();
            entities = entities.Where(x => x.Public == true || x.UserId == userId);
            return base.Filter(entities);
        }


        protected override async Task<Box> PostProcess(Box entity)
        {
            entity.UserId = User.GetId();
            return await base.PostProcess(entity);
        }

        protected override bool CanPost(int? id)
        {
            var userId = User.GetId();
            return !String.IsNullOrWhiteSpace(userId) && User.IsInRole(RoleConstants.Contributor) && OwnsBox(id, userId);
        }

        protected override bool CanDelete(int? id)
        {
            return CanPost(id);
        }

        private bool OwnsBox(int? id, string userId)
        {
            if (!id.HasValue || id == 0)
            {
                return true;
            }
            return DbContext.Boxes.Any(x => x.Id == id && x.UserId == userId);
        }

        protected override async Task<IQueryable<Box>> PerformSearch(IQueryable<Box> results, BoxSearchModel model)
        {
            results = PerformUserSearch(results, model.UserId);
            results = PerformBlacklistSearch(results, model.Blacklist);
            return results;
        }

        private IQueryable<Box> PerformBlacklistSearch(IQueryable<Box> results, bool? modelBlacklist)
        {
            if (modelBlacklist.HasValue)
            {
                results = results.Where(x => x.Blacklist == modelBlacklist);
            }
            return results;
        }

        private IQueryable<Box> PerformUserSearch(IQueryable<Box> results, string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                userId = User.GetId();
            }
            if (!String.IsNullOrWhiteSpace(userId))
            {
                results = results.Where(x => x.UserId == userId);
            }
            return results;
        }

        [HttpPost]
        [Authorize]
        [ActionName("Update")]
        [Route("[action]")]
        public async Task<IActionResult> Update([FromBody] BoxUpdateModel model)
        {
            if (!CanPost(model.Id))
            {
                return Unauthorized();
            }
            var box = await DbContext.Boxes.SingleOrDefaultAsync(x => x.Id == model.Id);
            if (box == null)
            {
                return BadRequest("Box not found.");
            }
            var seeked = await DbContext.Units.SingleOrDefaultAsync(x => x.Id == model.UnitId);
            if (seeked == null)
            {
                return BadRequest("Unit not found.");
            }
            if (model.Add)
            {
                if (!box.Units.Contains(seeked))
                {
                    box.Units.Add(seeked);
                }
            }
            else if (box.Units.Contains(seeked))
            {
                box.Units.Remove(seeked);
            }
            return Ok(1);
        }
    }
}
