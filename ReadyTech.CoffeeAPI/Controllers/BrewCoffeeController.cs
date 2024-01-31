using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReadyTech.CoffeeAPI.Controllers
{
    [Route("brew-coffee")]
    [ApiController]
    public class BrewCoffeeController : ControllerBase
    {
        public BrewCoffeeController() { }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
