using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Extensions;
using TreasureGuide.Web.Models.Teams;

namespace TreasureGuide.Web.Controllers
{
    public class TeamController : ApiController
    {
        private readonly TreasureEntities _entities;
        private readonly IMapper _mapper;

        public TeamController(IMapper mapper, TreasureEntities entities)
        {
            _mapper = mapper;
            _entities = entities;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id = null)
        {
            var results = _entities.Teams.AsQueryable();
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
        public async Task<IHttpActionResult> SaveTeam(SaveTeamModel model)
        {
            var team = await _entities.Teams.Import(model, _mapper, x => x.Id == model.Id);

            var unitList = new List<TeamUnit>();
            foreach (var newUnit in model.Units)
            {
                var unit = await _entities.TeamUnits.Import(model, _mapper, x => x.TeamId == model.Id && x.UnitId == newUnit.Id);
                unitList.Add(unit);
            }
            team.TeamUnits = unitList;

            var socketList = new List<TeamSocket>();
            foreach (var newSocket in model.Sockets)
            {
                var socket = await _entities.TeamSockets.Import(model, _mapper, x => x.TeamId == model.Id && x.SocketType == newSocket.SocketType);
                socketList.Add(socket);
            }
            team.TeamSockets = socketList;

            team.StageDifficulties = await _entities.StageDifficulties.Where(x => model.Stages.Contains(x.Id)).ToListAsync();
            await _entities.SaveChangesAsync();
            return Ok();
        }

        private async Task<IEnumerable<TeamBrowserModel>> CreateBrowserModel(IQueryable<Team> results)
        {
            var output = await results.Select(x => new TeamBrowserModel
            {
                Id = x.Id,
                Description = x.Description,
                Global = x.TeamUnits.All(y => y.Sub == false || y.Unit.UnitFlags.Any(z => z.FlagType == UnitFlagTypes.Global)),
                Units = x.TeamUnits.Where(y => y.Position <= 6 && y.Sub == false).Select(y => y.UnitId),
                TeamScore = x.TeamVotes.Count,
            }).OrderBy(x => x.TeamScore).ThenBy(x => x.Id).ToListAsync();
            return output;
        }
    }
}