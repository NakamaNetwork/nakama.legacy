using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Extensions;
using TreasureGuide.Web.Models.Stages;

namespace TreasureGuide.Web.Controllers
{
    public class StageController : ApiController
    {
        private readonly TreasureEntities _entities;
        private IMapper _mapper;

        public StageController(IMapper mapper, TreasureEntities entities)
        {
            _mapper = mapper;
            _entities = entities;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id = null)
        {
            var results = _entities.Stages.AsQueryable();
            if (id.HasValue)
            {
                results = results.Where(x => x.Id == id);
                if (!results.Any())
                {
                    return NotFound();
                }
            }
            var converted = await CreateBrowserModel(results);
            return Ok(converted);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveStage(SaveStageModel model)
        {
            var result = await _entities.Stages.Import(model, _mapper, x => x.Id == model.Id);
            await _entities.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveDifficulty(SaveDifficultyModel model)
        {
            var result = await _entities.Stages.Import(model, _mapper, x => x.Id == model.Id);
            await _entities.SaveChangesAsync();
            return Ok();
        }

        private async Task<IEnumerable<StageTypeModel>> CreateBrowserModel(IQueryable<Stage> results)
        {
            var grouped = results.GroupBy(x => x.Type);
            var output = await grouped.Select(x => new StageTypeModel
            {
                Id = (int)x.Key,
                Name = x.Key.ToString(),
                Stages = x.Select(y => new StageStubModel
                {
                    Id = y.Id,
                    Name = y.Name,
                    Global = y.Global,
                    Difficulties = y.StageDifficulties.Select(z => new StageDifficultyStubModel
                    {
                        Id = z.Id,
                        Name = z.Name,
                        Stamina = z.Stamina,
                        Global = z.Global,
                        Teams = z.Teams.Count
                    })
                })
            }).ToListAsync();
            return output;
        }
    }
}