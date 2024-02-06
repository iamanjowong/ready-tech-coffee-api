using Microsoft.Extensions.Options;

namespace ReadyTech.CoffeeAPI.Domain.OpenWeatherMap
{
    public sealed class OpenWeatherMapClient(HttpClient httpClient, IOptions<OpenWeatherMapOptions> option)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly OpenWeatherMapOptions _openWeatherMapOptions = option.Value;

        public async ValueTask<OpenWeatherMapResponse?> GetLatestWeatherAsync(string city)
        {
            try
            {
                var apiUrl = $"{_openWeatherMapOptions.Url}/2.5/weather?q={city}&appid={_openWeatherMapOptions.ApiKey}";
                var response = await _httpClient.GetFromJsonAsync<OpenWeatherMapResponse>(apiUrl);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving weather data: {ex.Message}");
                return null;
            }
        }
    }
}
