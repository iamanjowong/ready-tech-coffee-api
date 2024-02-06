namespace ReadyTech.CoffeeAPI.Domain.BrewCoffee
{
    public interface IGetBrewCoffeeHandler
    {
        ValueTask<GetBrewCoffeeResponse> HandleAsync();
    }
}
