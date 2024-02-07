using ReadyTech.CoffeeAPI.Infrastructure.HttpClients.OpenWeatherMap;
using ReadyTech.CoffeeAPI.Infrastructure.Providers;

namespace ReadyTech.CoffeeAPI.Domain.BrewCoffee
{
    public class GetBrewCoffeeHandler(IDateTimeProvider dateTimeProvider, OpenWeatherMapClient openWeatherMapClient) : IGetBrewCoffeeHandler
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly OpenWeatherMapClient _openWeatherMapClient = openWeatherMapClient;

        public async ValueTask<GetBrewCoffeeResponse> HandleAsync() 
        {
            var getCurrentWeather = await _openWeatherMapClient.GetLatestWeatherAsync("Sydney");

            if (getCurrentWeather != null && getCurrentWeather.Temperature > 30)
            {
                return new("Your refreshing iced coffee is ready", _dateTimeProvider.Now);
            }

            return new("Your piping hot coffee is ready", _dateTimeProvider.Now);
        }
    }
}
