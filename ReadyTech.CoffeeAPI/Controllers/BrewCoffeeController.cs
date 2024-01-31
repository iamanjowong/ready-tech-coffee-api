using Microsoft.AspNetCore.Mvc;
using ReadyTech.CoffeeAPI.Domain.BrewCoffee;

namespace ReadyTech.CoffeeAPI.Controllers
{
    [Route("brew-coffee")]
    [ApiController]
    public class BrewCoffeeController(IGetBrewCoffeeService brewCoffeeService) : ControllerBase
    {
        private readonly IGetBrewCoffeeService _brewCoffeeService = brewCoffeeService;

        [HttpGet]
        public ActionResult<GetBrewCoffeeResponse> Get()
        {
            var getBrewCoffeeResponse = _brewCoffeeService.GetBrewCoffeeResponse();

            return Ok(getBrewCoffeeResponse);
        }
    }
}
