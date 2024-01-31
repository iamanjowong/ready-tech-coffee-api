namespace ReadyTech.CoffeeAPI.Domain.BrewCoffee
{
    public class GetBrewCoffeeResponseBuilder : IGetBrewCoffeeResponseBuilder
    {
        public GetBrewCoffeeResponse Build() => new GetBrewCoffeeResponse("Your piping hot coffee is ready", DateTime.UtcNow);
    }
}
