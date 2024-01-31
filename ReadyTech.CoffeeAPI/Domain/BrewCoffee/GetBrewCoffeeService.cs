namespace ReadyTech.CoffeeAPI.Domain.BrewCoffee
{
    public class GetBrewCoffeeService : IGetBrewCoffeeService
    {
        public GetBrewCoffeeResponse GetBrewCoffeeResponse()
        {
            return new GetBrewCoffeeResponse("Your piping hot coffee is ready", DateTime.Now);
        }
    }
}
