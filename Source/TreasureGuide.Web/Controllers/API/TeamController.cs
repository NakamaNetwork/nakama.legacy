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

            teams = SearchTerm(teams, model.Team);
            teams = SearchStage(teams, model.Stage);
            teams = SearchLead(teams, model.Leader);
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

            }
            return teams;
        }

        private IQueryable<Team> SearchStage(IQueryable<Team> teams, string stage)
        {
            if (!String.IsNullOrEmpty(stage))
            {

            }
            return teams;
        }

        private IQueryable<Team> SearchLead(IQueryable<Team> teams, string leader)
        {
            if (!String.IsNullOrEmpty(leader))
            {

            }
            return teams;
        }

        private IQueryable<Team> SearchGlobal(IQueryable<Team> teams, bool global)
        {
            if (global)
            {

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
