using System;
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
        private readonly IPreferenceService _preferenceService;

        public BoxController(IPreferenceService preferenceService, TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
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
        [ActionName("Focus")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Focus(int? id)
        {
            await _preferenceService.SetPreference(User.GetId(), UserPreferenceType.BoxId, id?.ToString());
            return await Detail(id);
        }

        [HttpPost]
        [Authorize]
        [ActionName("Update")]
        [Route("[action]")]
        public async Task<IActionResult> Update([FromBody] BoxUpdateModel model)
        {
            return await BulkOperation(model, false);
        }

        [HttpPost]
        [Authorize]
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
                using (var transaction = DbContext.Database.BeginTransaction())
                {
                    if (clear)
                    {
                        var command = String.Format("DELETE FROM [dbo].[BoxUnits] WHERE [BoxId] = {0}", model.Id);
                        await DbContext.Database.ExecuteSqlCommandAsync(TransactionalBehavior.EnsureTransaction, command);
                    }
                    else if (model.Removed?.Any() ?? false)
                    {
                        var command = String.Format(
                            "DELETE FROM [dbo].[BoxUnits] WHERE [BoxId] = {0} AND [UnitId] IN ({1})", model.Id,
                            String.Join(",", model.Removed));
                        await DbContext.Database.ExecuteSqlCommandAsync(TransactionalBehavior.EnsureTransaction, command);
                    }
                    if (model.Added?.Any() ?? false)
                    {
                        var existing = box.Units.Select(x => x.Id).ToList();
                        var real = await DbContext.Units.Where(x => model.Added.Contains(x.Id)).Select(x => x.Id).ToListAsync();
                        var actual = real.Except(existing);
                        if (actual.Any())
                        {
                            var commands = actual.Select(x => $"INSERT INTO [dbo].[BoxUnits]([BoxId],[UnitId]) VALUES({model.Id},{x})");
                            var collection = String.Join(";", commands);
                            await DbContext.Database.ExecuteSqlCommandAsync(TransactionalBehavior.EnsureTransaction, collection);
                        }
                    }
                    transaction.Commit();
                }
            }
            return Ok(1);
        }
    }
}
