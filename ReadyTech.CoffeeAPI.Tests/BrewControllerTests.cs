using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

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
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}