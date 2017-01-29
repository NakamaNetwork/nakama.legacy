using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Controllers
{
    public class GuideController : ApiController
    {
        private readonly TreasureEntities _entities;

        public GuideController(TreasureEntities entities)
        {
            _entities = entities;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Team(int id)
        {
            var result = await _entities.Teams.SingleOrDefaultAsync(x => x.Id == id);
            return Ok(result);
        }
    }
}