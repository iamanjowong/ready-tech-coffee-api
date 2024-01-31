using ReadyTech.CoffeeAPI.Infrastructure;

namespace ReadyTech.CoffeeAPI.Domain.BrewCoffee
{
    public class GetBrewCoffeeResponseBuilder(IDateTimeProvider dateTimeProvider) : IGetBrewCoffeeResponseBuilder
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

        public GetBrewCoffeeResponse Build() => new("Your piping hot coffee is ready", _dateTimeProvider.Now);
    }
}
