using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Models.GCRModels;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize(Roles = RoleConstants.GlobalClearRates)]
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
                Teams = teams.Select(x => new GCRTableModel
                {
                    Id = x.Id,
                    LeaderId = x.LeaderId,
                    StageId = x.StageId ?? 0,
                    F2P = x.F2P ?? false,
                    Global = x.Global ?? false,
                    Video = x.Video ?? false
                })
            };
            return Ok(result);
        }
    }
}
