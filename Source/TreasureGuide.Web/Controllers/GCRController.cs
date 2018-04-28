using System;
using System.Collections.Generic;
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
    [Authorize(Roles = RoleConstants.GCRViewer)]
    public class GCRController : Controller
    {
        private readonly TreasureEntities _entities;

        public GCRController(TreasureEntities entities)
        {
            _entities = entities;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var teams = await _entities.GCRTables.ToListAsync();
            var units = await _entities.GCRUnitInfoes.OrderBy(x => x.Order).Select(x => new GCRDataModel
            {
                Id = x.UnitId,
                Name = x.Name,
                Thumbnail = x.UnitId,
            }).ToListAsync();
            units.Add(new GCRDataModel
            {
                Id = null,
                Name = "F2P"
            });
            var stages = await _entities.GCRStageInfoes.OrderBy(x => x.Order).Select(x => new GCRDataModel
            {
                Id = x.StageId,
                Name = x.Name,
                Thumbnail = x.Thumbnail
            }).ToListAsync();
            var result = new GCRResultModel
            {
                Units = units,
                Stages = stages,
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

        [HttpGet]
        public async Task<IActionResult> Missing()
        {
            var result = await GetMissingData();
            var resultString = ConvertToContent(result);
            return Content(resultString);
        }

        private async Task<IDictionary<GCRDataModel, List<GCRDataModel>>> GetMissingData()
        {
            var safeUnits = await _entities.GCRTables
                .Where(x => (x.F2P ?? false) && (x.Global ?? false) && (x.Video ?? false))
                .Select(x => new { x.StageId, x.LeaderId })
                .ToListAsync();

            var units = await _entities.GCRUnitInfoes.OrderBy(x => x.Order).Select(x => new GCRDataModel
            {
                Id = x.UnitId,
                Name = x.Name,
                Thumbnail = x.UnitId,
                Color = x.Color
            }).ToListAsync();
            units.Add(new GCRDataModel
            {
                Id = null,
                Name = "F2P"
            });
            var stages = await _entities.GCRStageInfoes.OrderBy(x => x.Order).Select(x => new GCRDataModel
            {
                Id = x.StageId,
                Name = x.Name,
                Thumbnail = x.Thumbnail,
                Color = x.Color
            }).ToListAsync();
            var result = stages
                .ToDictionary(x => x, x => units.Where(y => !safeUnits.Any(z => z.StageId == x.Id && z.LeaderId == y.Id)).ToList());
            return result;
        }

        private string ConvertToContent(IDictionary<GCRDataModel, List<GCRDataModel>> result)
        {
            var header = "";
            var separator = "";
            var body = "";
            var max = result.Max(x => x.Value.Count());
            for (var i = 0; i < max; i++)
            {
                foreach (var missing in result)
                {
                    if (i == 0)
                    {
                        if (missing.Key.Color.HasValue)
                        {
                            header += $"[{missing.Key.Name}](/{missing.Key.Color})|";
                        }
                        else
                        {
                            header += missing.Key.Name;
                        }
                        header += "|";
                        separator += "-|";
                    }
                    if (missing.Value.Count > i)
                    {
                        var leader = missing.Value[i];
                        if (leader.Color.HasValue)
                        {
                            body += $"[{leader.Name}](/{leader.Color})";
                        }
                        else
                        {
                            body += leader.Name;
                        }
                    }
                    body += "|";
                }
                body += "\r\n";
            }
            return String.Join("\r\n", header, separator, body);
        }
    }
}
