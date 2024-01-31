using Microsoft.AspNetCore.Mvc;
using ReadyTech.CoffeeAPI.Domain.BrewCoffee;

namespace ReadyTech.CoffeeAPI.Controllers
{
    [Route("brew-coffee")]
    [ApiController]
    public class BrewCoffeeController(IGetBrewCoffeeResponseBuilder brewCoffeeResponseBuilder) : ControllerBase
    {
        private readonly IGetBrewCoffeeResponseBuilder _brewCoffeeResponseBuilder = brewCoffeeResponseBuilder;

        [HttpGet]
        public ActionResult<GetBrewCoffeeResponse> Get()
        {
            var getBrewCoffeeResponse = _brewCoffeeResponseBuilder.Build();

            return Ok(getBrewCoffeeResponse);
        }
    }
}
