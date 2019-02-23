using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NakamaNetwork.Entities.Models;
using System.Threading.Tasks;
using TreasureGuide.Common.Models.TeamModels;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Services.QueryService;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/team")]
    public class TeamController : SearchableDatabaseApiController<Team, TeamSearchModel, ITeamDatabaseQueryService>
    {
        public TeamController(ITeamDatabaseQueryService queryService) : base(queryService)
        {
        }

        [HttpGet]
        [ActionName("")]
        [Route("[action]")]
        [EnableCors("NakamaCORS")]
        public async Task<IActionResult> Get(int? id = null)
        {
            var results = await QueryService.FindAsync(id);
            return Ok(results);
        }

        [HttpGet]
        [ActionName("Wiki")]
        [Route("[action]")]
        [EnableCors("NakamaCORS")]
        public async Task<IActionResult> Wiki(TeamSearchModel model)
        {
            var results = await QueryService.SearchWiki(model).ToListAsync();
            return Ok(results);
        }

        [HttpGet]
        [ActionName("Trending")]
        [Route("[action]")]
        public async Task<IActionResult> Trending()
        {
            var results = await QueryService.GetTrending().ToListAsync();
            return Ok(results);
        }

        [HttpGet]
        [ActionName("Latest")]
        [Route("[action]")]
        public async Task<IActionResult> Latest()
        {
            var results = await QueryService.GetLatest().ToListAsync();
            return Ok(results);
        }
    }
}