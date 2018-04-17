using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Models.GCRModels;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize(Roles = RoleConstants.GCRAdmin)]
    [Route("api/gcr")]
    public class GCRController : Controller
    {
        private readonly TreasureEntities _entities;
        private readonly IMapper _mapper;

        public GCRController(TreasureEntities entities, IMapper mapper)
        {
            _entities = entities;
            _mapper = mapper;
        }

        [HttpGet]
        [ActionName("Get")]
        [Route("")]
        [Route("[action]")]
        public async Task<IActionResult> Get()
        {
            var units = await _entities.GCRUnits.OrderBy(x => x.Order).ProjectTo<GCRUnitEditModel>(_mapper.ConfigurationProvider).ToListAsync();
            var stages = await _entities.GCRStages.OrderBy(x => x.Order).ProjectTo<GCRStageEditModel>(_mapper.ConfigurationProvider).ToListAsync();
            var result = new GCREditorModel
            {
                Units = units,
                Stages = stages
            };
            return Ok(result);
        }

        [HttpPost]
        [ActionName("Units")]
        [Route("[action]")]
        public async Task<IActionResult> UnitPost([FromBody]IEnumerable<GCRUnitEditModel> model)
        {
            _entities.GCRUnits.Clear();

            var units = model.Select(x => _mapper.Map<GCRUnit>(x));
            _entities.GCRUnits.AddRange(units);

            await _entities.SaveChangesAsync();
            return Ok(1);
        }

        [HttpPost]
        [ActionName("Stages")]
        [Route("[action]")]
        public async Task<IActionResult> StagePost([FromBody]IEnumerable<GCRStageEditModel> model)
        {
            _entities.GCRStages.Clear();

            var stages = model.Select(x => _mapper.Map<GCRStage>(x));
            _entities.GCRStages.AddRange(stages);

            await _entities.SaveChangesAsync();
            return Ok(1);
        }
    }
}
