using Microsoft.Extensions.Caching.Memory;

namespace ReadyTech.CoffeeAPI.Infrastructure.Middleware
{
    public class ServiceUnavailableEnablerMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        private readonly RequestDelegate _next = next;
        private readonly IMemoryCache _cache = cache;
        private const string CallCounterKey = "CallCounter";

        public async Task InvokeAsync(HttpContext context)
        {
            var callCounter = _cache.GetOrCreate(CallCounterKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return 0;
            });

            callCounter++;
            _cache.Set(CallCounterKey, callCounter);

            if (callCounter % 5 == 0)
            {
                context.Response.StatusCode = 503;
                return;
            }

            await _next(context);
        }
    }
}
