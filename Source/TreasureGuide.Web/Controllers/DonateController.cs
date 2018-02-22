using Microsoft.AspNetCore.Mvc;

namespace TreasureGuide.Web.Controllers
{
    public class DonateController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
