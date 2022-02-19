using Microsoft.AspNetCore.Mvc;

namespace CartAPI.Controllers
{
    [Route("api/[controller]")]
    public class CatsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetCats()
        {
            return Ok("miau");
        }
    }
}
