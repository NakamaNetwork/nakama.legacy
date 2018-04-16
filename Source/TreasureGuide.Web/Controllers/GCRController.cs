using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Models.GCRModels;

namespace TreasureGuide.Web.Controllers
{
    [Authorize(Roles = RoleConstants.GlobalClearRates)]
    [Route("gcr")]
    public class GCRController : Controller
    {
        private readonly TreasureEntities _entities;

        public GCRController(TreasureEntities entities)
        {
            _entities = entities;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _entities.GCRTables.ToListAsync();
            var units = await _entities.GCRUnits.OrderBy(x => x.Order).Select(x => x.UnitId)
                .Join(_entities.Units, x => x, y => y.Id, (x, y) => new GCRDataModel
                {
                    Id = y.Id,
                    Name = y.Name,
                    Thumbnail = y.Id,
                }).ToListAsync();
            units.Add(new GCRDataModel
            {
                Id = null,
                Name = "F2P"
            });
            var stages = await _entities.GCRStages.OrderBy(x => x.Order).Select(x => x.StageId)
                .Join(_entities.Stages, x => x, y => y.Id, (x, y) => new GCRDataModel
                {
                    Id = y.Id,
                    Name = y.Name,
                    Thumbnail = y.UnitId
                }).ToListAsync();
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
            return View(result);
        }
    }
}
