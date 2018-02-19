using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
        private readonly IPreferenceService _preferenceService;

        public BoxController(IPreferenceService preferenceService, TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
            Throttled = true;
            _preferenceService = preferenceService;
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
            return !String.IsNullOrWhiteSpace(userId) &&
                (User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator)
                || (User.IsInRole(RoleConstants.BoxUser) && (id.HasValue ? OwnsBox(id, userId) : UnderBoxLimit(userId))));
        }

        protected override bool CanDelete(int? id)
        {
            return CanPost(id);
        }

        private bool UnderBoxLimit(string userId)
        {
            var limit = User.IsInRole(RoleConstants.MultiBoxUser)
                ? BoxConstants.MultiBoxUserLimit
                : BoxConstants.BoxUserLimit;
            return DbContext.Boxes.Count(x => x.UserId == userId) < limit;
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
        [ActionName("Focus")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Focus(int? id)
        {
            if (id.HasValue && User.IsInRole(RoleConstants.BoxUser))
            {
                if (Throttled && !ThrottlingService.CanAccess(User, Request))
                {
                    return StatusCode((int)HttpStatusCode.Conflict, ThrottleService.Message);
                }
                await _preferenceService.SetPreference(User.GetId(), UserPreferenceType.BoxId, id?.ToString());
                return await Detail(id);
            }
            else
            {
                await _preferenceService.ClearPreference(User.GetId(), UserPreferenceType.BoxId);
                return Ok(1);
            }
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.BoxUser)]
        [ActionName("Update")]
        [Route("[action]")]
        public async Task<IActionResult> Update([FromBody] BoxUpdateModel model)
        {
            return await BulkOperation(model, false);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.BoxUser)]
        [ActionName("Set")]
        [Route("[action]")]
        public async Task<IActionResult> Set([FromBody] BoxUpdateModel model)
        {
            return await BulkOperation(model, true);
        }

        private async Task<IActionResult> BulkOperation(BoxUpdateModel model, bool clear)
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
            if ((model.Added?.Any() ?? false) || (model.Removed?.Any() ?? false))
            {
                if (clear)
                {
                    DbContext.BoxUnits.RemoveRange(DbContext.BoxUnits.Where(x => x.BoxId == model.Id));
                }
                else if (model.Removed?.Any() ?? false)
                {
                    DbContext.BoxUnits.RemoveRange(DbContext.BoxUnits.Where(x => x.BoxId == model.Id && model.Removed.Contains(x.UnitId)));
                }
                if (model.Added?.Any() ?? false)
                {
                    var existing = box.BoxUnits.Select(x => x.UnitId).ToList();
                    var real = await DbContext.Units.Where(x => model.Added.Contains(x.Id)).Select(x => x.Id).ToListAsync();
                    var actual = real.Except(existing);
                    if (actual.Any())
                    {
                        var newItems = actual.Select(x => new BoxUnit
                        {
                            BoxId = model.Id,
                            UnitId = x
                        });
                        DbContext.BoxUnits.AddRange(newItems);
                    }
                }
                await DbContext.SaveChangesAsync();
            }
            return Ok(1);
        }
    }
}
