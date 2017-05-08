using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.TeamModels;

namespace TreasureGuide.Web.Controllers.API
{
    public class TeamController : EntityApiController<int, Team, TeamStubModel, TeamDetailModel, TeamEditorModel>
    {
        private const int MAX_DEPTH = 10;

        public TeamController(TreasureEntities dbContext, IMapper autoMapper) : base(dbContext, autoMapper)
        {
        }

        protected override Team PostProcess(Team entity)
        {
            entity.SubmittedById = "Anonymous";
            return base.PostProcess(entity);
        }

        [HttpGet]
        [ActionName("Search")]
        [Route("[action]")]
        public async Task<IEnumerable<TeamStubModel>> Search(TeamSearchModel model)
        {
            model = model ?? new TeamSearchModel();
            var teams = DbContext.Teams.AsQueryable();

            teams = SearchStage(teams, model.StageId);
            teams = SearchTerm(teams, model.Team);
            teams = SearchLead(teams, model.LeaderId);
            teams = SearchGlobal(teams, model.Global);
            teams = SearchBox(teams, model.MyBox);

            teams = teams.OrderBy(x => x.TeamVotes.Count).Skip(model.PageSize * model.Page).Take(model.Page);
            var output = teams.ProjectTo<TeamStubModel>(AutoMapper.ConfigurationProvider);
            return await output.ToListAsync();
        }

        private IQueryable<Team> SearchTerm(IQueryable<Team> teams, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                teams = teams.Where(x => x.Name.Contains(term));
            }
            return teams;
        }

        private IQueryable<Team> SearchStage(IQueryable<Team> teams, int? stageId)
        {
            if (stageId.HasValue)
            {
                var stages = DbContext.Stages.Where(x => x.Id == stageId);
                var depth = 0;
                var teamIds = Enumerable.Empty<int>();
                while (stages.Any() && depth < MAX_DEPTH)
                {
                    teamIds = teamIds.Concat(stages.Select(x => x.Id));
                    stages = stages.SelectMany(x => x.ChildStages);
                }
                teams = teams.Join(teamIds, x => x.Id, y => y, (x, y) => x);
            }
            return teams;
        }

        private IQueryable<Team> SearchLead(IQueryable<Team> teams, int? leaderId)
        {
            if (leaderId.HasValue)
            {
                teams = teams.Where(x => x.TeamUnits.Any(y => y.UnitId == leaderId && y.Position < 2));
            }
            return teams;
        }

        private IQueryable<Team> SearchGlobal(IQueryable<Team> teams, bool global)
        {
            if (global)
            {
                teams = teams.Where(x => x.TeamUnits.All(y => y.Sub || y.Unit.UnitFlags.Any(z => z.FlagType == UnitFlagType.Global)));
            }
            return teams;
        }

        private IQueryable<Team> SearchBox(IQueryable<Team> teams, bool myBox)
        {
            if (myBox)
            {

            }
            return teams;
        }
    }
}
