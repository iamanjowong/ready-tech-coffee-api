using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ReadyTech.CoffeeAPI.Domain.BrewCoffee;
using ReadyTech.CoffeeAPI.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace ReadyTech.CoffeeAPI.Tests
{
    public class BrewControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string BREW_COFFEE_URL = "/brew-coffee";

        private readonly WebApplicationFactory<Program> _factory = factory;

        [Fact]
        public async Task ShouldReturnSuccess()
        {
            // Arrange
            var mockDateTimeProvider = DateTimeProvider(new DateTime(2023, 4, 2));
            var dateTimeProvider = mockDateTimeProvider.Object;
            var client = CreateClientWithOverrides(services =>
            {
                services.AddSingleton(dateTimeProvider);
            });

            // Act
            var response = await client.GetAsync(BREW_COFFEE_URL);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.NotNull(response);
            Assert.NotNull(response.Content);

            var getBrewCoffeeResponse = await response.Content.ReadFromJsonAsync<GetBrewCoffeeResponse>();

            Assert.NotNull(getBrewCoffeeResponse);
            Assert.Equal("Your piping hot coffee is ready", getBrewCoffeeResponse.Message);
            Assert.Equal(dateTimeProvider.Now, getBrewCoffeeResponse.Prepared);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task WhenItsOnEveryFifthCall_ShouldReturnServiceUnavailable()
        {
            // Arrange
            var client = _factory.CreateClient();
            var serviceUnavailableCounter = 0;

            // Act & Assert
            for (var i = 1; i <= 10; i++)
            {
                var response = await client.GetAsync(BREW_COFFEE_URL);
                var content = await response.Content.ReadAsStringAsync();

                if(i % 5 == 0)
                {
                    Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
                    Assert.Equal(string.Empty, content);
                    serviceUnavailableCounter++;
                    continue;
                }

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotEqual(string.Empty, content);
            }

            Assert.Equal(2, serviceUnavailableCounter);
        }

        [Fact]
        public async Task WhenDateIsAprilFirst_ShouldReturn418()
        {
            // Arrange
            var mockDateTimeProvider = DateTimeProvider(new DateTime(2023, 4, 1));
            var client = CreateClientWithOverrides(services =>
            {
                services.AddSingleton(mockDateTimeProvider.Object);
            });

            // Act
            var response = await client.GetAsync(BREW_COFFEE_URL);

            // Assert
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("418", response.StatusCode.ToString());
            Assert.Equal(string.Empty, content);
        }

        [Fact]
        public async Task WhenDateIsNotAprilFirst_ShouldNotReturn418()
        {
            // Arrange
            var mockDateTimeProvider = DateTimeProvider(new DateTime(2023, 4, 2));
            var client = CreateClientWithOverrides(services =>
            {
                services.AddSingleton(mockDateTimeProvider.Object);
            });

            // Act
            var response = await client.GetAsync(BREW_COFFEE_URL);

            // Assert
            var content = await response.Content.ReadAsStringAsync();

            Assert.NotEqual("418", response.StatusCode.ToString());
        }

        private static Mock<IDateTimeProvider> DateTimeProvider(DateTime customDateTime)
        {
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(provider => provider.Now)
                                .Returns(customDateTime);

            return mockDateTimeProvider;
        }

        private HttpClient CreateClientWithOverrides(Action<IServiceCollection> serviceOverrides) => 
            _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(serviceOverrides);
            }).CreateClient();
    }
}