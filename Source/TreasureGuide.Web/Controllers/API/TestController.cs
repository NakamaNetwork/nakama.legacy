using Microsoft.AspNetCore.Mvc;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get(int? id = null)
        {
            return Ok($"Get {id}");
        }

        [HttpPut]
        public IActionResult Put(int? id = null)
        {
            return Ok($"Put {id}");
        }

        [HttpPost]
        public IActionResult Post(int? id = null)
        {
            return Ok($"Post {id}");
        }

        [HttpDelete]
        public IActionResult Delete(int? id = null)
        {
            return Ok($"Delete {id}");
        }
    }
}
