using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.UnitModels;

namespace TreasureGuide.Web.Controllers.API
{
    public class UnitController : EntityApiController<int, Unit, UnitStubModel, UnitDetailModel, UnitEditorModel>
    {
        public UnitController(TreasureEntities dbContext, IMapper autoMapper) : base(dbContext, autoMapper)
        {
        }
    }
}
