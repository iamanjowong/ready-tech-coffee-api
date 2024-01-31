using Microsoft.AspNetCore.Mvc.Testing;
using ReadyTech.CoffeeAPI.Domain.BrewCoffee;
using System.Net;
using System.Net.Http.Json;

namespace ReadyTech.CoffeeAPI.Tests
{
    public class BrewControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        public const string BREW_COFFEE_URL = "/brew-coffee";

        protected readonly WebApplicationFactory<Program> _factory = factory;

        [Fact]
        public async Task WhenGetBrewCoffeeIsCalled_ShouldReturnSuccess()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(BREW_COFFEE_URL);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
            var getBrewCoffeeResponse = await response.Content.ReadFromJsonAsync<GetBrewCoffeeResponse>();
            Assert.NotNull(getBrewCoffeeResponse);
            Assert.Equal("Your piping hot coffee is ready", getBrewCoffeeResponse.Message);
            Assert.NotEqual(default, getBrewCoffeeResponse.Prepared);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}