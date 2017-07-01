using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProfileContoller : Controller
    {
        [HttpGet]
        [Route("")]
        [ActionName("")]
        public IActionResult Get()
        {
            return Ok("");
        }
    }
}
