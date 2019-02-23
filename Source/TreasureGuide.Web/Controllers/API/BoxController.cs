using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Common.Constants;
using NakamaNetwork.Entities.Models;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Common.Helpers;
using TreasureGuide.Common.Models.BoxModels;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Services;
using NakamaNetwork.Entities.EnumTypes;
using Microsoft.EntityFrameworkCore;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/box")]
    public class BoxController : SearchableApiController<int, Box, int?, BoxStubModel, BoxDetailModel, BoxEditorModel, BoxSearchModel>
    {
        private readonly IPreferenceService _preferenceService;

        public BoxController(IPreferenceService preferenceService, NakamaNetworkContext dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
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
            var limit = User.IsInRole(RoleConstants.Donor)
                ? BoxConstants.DonorBoxLimit
                : BoxConstants.BoxLimit;
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
            if (Throttled && !ThrottlingService.CanAccess(User, Request, Request.Path))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            if (id.HasValue && User.IsInRole(RoleConstants.BoxUser))
            {
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

        [HttpPost]
        [Authorize(Roles = RoleConstants.BoxUser)]
        [ActionName("Flags")]
        [Route("[action]")]
        public async Task<IActionResult> Flags([FromBody] BoxUpdateModel model)
        {
            return await BulkOperation(model, false);
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
            model.Added = model.Added ?? Enumerable.Empty<int>();
            model.Removed = model.Removed ?? Enumerable.Empty<int>();
            model.Updated = model.Updated ?? Enumerable.Empty<BoxUnitUpdateModel>();
            if (model.Added.Any() || model.Removed.Any() || model.Updated.Any())
            {
                if (clear)
                {
                    DbContext.BoxUnits.RemoveWhere(x => x.BoxId == model.Id && !model.Added.Contains(x.UnitId));
                }
                else if (model.Removed.Any())
                {
                    DbContext.BoxUnits.RemoveWhere(x => x.BoxId == model.Id && model.Removed.Contains(x.UnitId));
                }
                if (model.Added.Any())
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
                if (model.Updated.Any())
                {
                    var updates = model.Updated.Select(x => new BoxUnit { BoxId = model.Id, UnitId = x.Id, Flags = x.Flags });
                    DbContext.BoxUnits.AttachRange(updates);
                }
                await DbContext.SaveChangesSafe();
            }
            return Ok(1);
        }
    }
}
