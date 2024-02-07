using System.Text.Json.Serialization;

namespace ReadyTech.CoffeeAPI.Domain.BrewCoffee
{
    public record GetBrewCoffeeResponse(string Message, [property: JsonPropertyName("prepared")]DateTime PreparedDate);
}
