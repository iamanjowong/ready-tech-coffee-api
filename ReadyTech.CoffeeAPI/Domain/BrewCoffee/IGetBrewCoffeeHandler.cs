namespace ReadyTech.CoffeeAPI.Domain.BrewCoffee
{
    public interface IGetBrewCoffeeHandler
    {
        Task<GetBrewCoffeeResponse> HandleAsync();
    }
}
