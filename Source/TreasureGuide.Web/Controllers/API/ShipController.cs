using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.ShipModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/ship")]
    public class ShipController : LocallyCachedController<int, Ship, ShipStubModel>
    {
        public ShipController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(DeletedItemType.Ship, dbContext, autoMapper, throttlingService)
        {
        }
    }
}
