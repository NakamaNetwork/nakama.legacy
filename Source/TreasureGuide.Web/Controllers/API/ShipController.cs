using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.ShipModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    public class ShipController : EntityApiController<int, Ship, int?, ShipStubModel, ShipDetailModel, ShipEditorModel>
    {
        public ShipController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }
    }
}
