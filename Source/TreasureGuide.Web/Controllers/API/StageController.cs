using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.StageModels;

namespace TreasureGuide.Web.Controllers.API
{
    public class StageController : EntityApiController<int, Stage, StageStubModel, StageDetailModel, StageEditorModel>
    {
        public StageController(TreasureEntities dbContext, IMapper autoMapper) : base(dbContext, autoMapper)
        {
        }
    }
}
