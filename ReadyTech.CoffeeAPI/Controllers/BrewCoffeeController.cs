using Microsoft.AspNetCore.Mvc;
using ReadyTech.CoffeeAPI.Domain.BrewCoffee;

namespace ReadyTech.CoffeeAPI.Controllers
{
    [Route("brew-coffee")]
    [ApiController]
    public class BrewCoffeeController(IGetBrewCoffeeHandler brewCoffeeResponseBuilder) : ControllerBase
    {
        private readonly IGetBrewCoffeeHandler _getBrewCoffeeHandler = brewCoffeeResponseBuilder;

        [HttpGet]
        public async Task<ActionResult<GetBrewCoffeeResponse>> Get()
        {
            var getBrewCoffeeResponse = await _getBrewCoffeeHandler.HandleAsync();

            return Ok(getBrewCoffeeResponse);
        }
    }
}
