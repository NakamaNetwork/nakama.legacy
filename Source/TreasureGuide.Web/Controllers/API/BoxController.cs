using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models.BoxModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/box")]
    public class BoxController : EntityApiController<int, Box, int?, BoxStubModel, BoxDetailModel, BoxEditorModel>
    {
        public BoxController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override bool CanPost(int? id)
        {
            var userId = User.GetId();
            return !String.IsNullOrWhiteSpace(userId) && OwnsBox(id, userId);
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
