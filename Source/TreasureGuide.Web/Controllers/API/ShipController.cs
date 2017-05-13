using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.ShipModels;

namespace TreasureGuide.Web.Controllers.API
{
    public class ShipController : EntityApiController<int, Ship, ShipStubModel, ShipDetailModel, ShipEditorModel>
    {
        public ShipController(TreasureEntities dbContext, IMapper autoMapper) : base(dbContext, autoMapper)
        {
        }
    }
}
