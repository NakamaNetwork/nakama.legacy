using AutoMapper;
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
    }
}
