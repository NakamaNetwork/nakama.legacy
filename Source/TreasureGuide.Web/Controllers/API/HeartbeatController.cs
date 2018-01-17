using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Web.Helpers;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize]
    [Route("api/heartbeat")]
    public class HeartbeatController : Controller
    {
        [HttpGet]
        [ActionName("")]
        [Route("")]
        public IActionResult Get()
        {
            if (String.IsNullOrWhiteSpace(User.GetId()))
            {
                return Unauthorized();
            }
            return Ok(1);
        }
    }
}
