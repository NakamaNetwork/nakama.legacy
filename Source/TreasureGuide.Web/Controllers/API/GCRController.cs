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
using TreasureGuide.Web.Models.GCRModels;
using TreasureGuide.Web.Services;
using Z.EntityFramework.Plus;

namespace TreasureGuide.Web.Controllers.API
{
    //[Authorize]
    [Route("api/gcr")]
    public class GCRController : Controller
    {
        private readonly TreasureEntities _entities;

        public GCRController(TreasureEntities entities)
        {
            _entities = entities;
        }

        [HttpGet]
        [ActionName("")]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var teams = await _entities.GCRTables.ToListAsync();
            var units = await _entities.GCRUnits.OrderBy(x => x.Order).Select(x => x.UnitId).ToListAsync();
            var stages = await _entities.GCRStages.OrderBy(x => x.Order).Select(x => x.StageId).ToListAsync();
            var result = new GCRResultModel
            {
                UnitIds = units,
                StageIds = stages,
                Teams = teams
            };
            return Ok(result);
        }
    }
}
