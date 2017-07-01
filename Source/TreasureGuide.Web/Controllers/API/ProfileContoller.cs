using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Web.Models.ProfileModels;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProfileContoller : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name);
            var roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value);
            var model = new ProfileModel
            {
                EmailAddress = email,
                UserName = name,
                Roles = roles
            };
            return Ok(model);
        }
    }
}
