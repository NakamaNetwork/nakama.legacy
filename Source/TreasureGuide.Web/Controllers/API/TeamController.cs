using System;
using System.Linq;
using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.TeamModels;

namespace TreasureGuide.Web.Controllers.API
{
    public class TeamController : SearchableApiController<int, Team, TeamStubModel, TeamDetailModel, TeamEditorModel, TeamSearchModel>
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

        protected override IQueryable<Team> PerformSearch(IQueryable<Team> results, TeamSearchModel model)
        {
            results = SearchStage(results, model.StageId);
            results = SearchTerm(results, model.Term);
            results = SearchLead(results, model.LeaderId);
            results = SearchGlobal(results, model.Global);
            results = SearchBox(results, model.MyBox);
            return results;
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
