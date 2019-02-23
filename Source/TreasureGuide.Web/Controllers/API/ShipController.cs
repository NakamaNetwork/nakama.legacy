using Microsoft.AspNetCore.Mvc;
using NakamaNetwork.Entities.Models;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/ship")]
    public class ShipController : LocallyCachedController<Ship>
    {
        public ShipController(NakamaNetworkContext dbContext, IThrottleService throttlingService) : base(dbContext, throttlingService)
        {
        }
    }
}
