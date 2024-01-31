using ReadyTech.CoffeeAPI.Infrastructure.Providers;

namespace ReadyTech.CoffeeAPI.Infrastructure.Middleware
{
    public class TeapotMiddleware(RequestDelegate next, IDateTimeProvider dateTimeProvider)
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (_dateTimeProvider.Now.Month == 4 && _dateTimeProvider.Now.Day == 1)
            {
                context.Response.StatusCode = 418;
                return;
            }

            await _next(context);
        }
    }
}
