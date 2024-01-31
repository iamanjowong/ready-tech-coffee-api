namespace ReadyTech.CoffeeAPI.Infrastructure
{
    public static class BrewCoffeeMiddlewareExtensions
    {
        public static IApplicationBuilder UseBrewCoffeeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ServiceUnavailableEnablerMiddleware>()
                        .UseMiddleware<TeapotMiddleware>();
        }

        public static ControllerActionEndpointConventionBuilder MapBrewCoffeeEndpoint(this IEndpointRouteBuilder endpoints, string pattern)
        {
            _ = endpoints.CreateApplicationBuilder()
                .UseMiddleware<ServiceUnavailableEnablerMiddleware>()
                .UseMiddleware<TeapotMiddleware>()
                .Build();

            return endpoints.MapControllerRoute(name: "brew-coffee", pattern: pattern);
        }
    }
}
