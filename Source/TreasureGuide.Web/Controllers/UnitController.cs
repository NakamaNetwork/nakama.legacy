using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Models.Units;

namespace TreasureGuide.Web.Controllers
{
    public class UnitController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly TreasureEntities _entities;

        public UnitController(TreasureEntities entities, IMapper mapper)
        {
            _entities = entities;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id = null)
        {
            var results = _entities.Units.AsQueryable();
            if (id.HasValue)
            {
                results = results.Where(x => x.Id == null);
                if (!results.Any())
                {
                    return NotFound();
                }
            }
            var converted = await CreateBrowserModel(results);
            return Ok(converted);
        }

        private async Task<IEnumerable<UnitStubModel>> CreateBrowserModel(IQueryable<Unit> results)
        {
            var output = await results.Select(x => new UnitStubModel
            {
                Id = x.Id,
                Name = x.Name,
                Stars = x.Stars,
                Type = x.Type ?? UnitType.Unknown,
                Sockets = x.Sockets,
                Classes = x.UnitClasses.Select(y => y.Class),
                Flags = x.UnitFlags.Select(y => y.FlagType),
                LeadTeams = x.TeamUnits.Count(y => y.Position <= 2),
                OnTeams = x.TeamUnits.Count()
            }).OrderBy(x => x.Id).ToListAsync();
            return output;
        }
    }
}