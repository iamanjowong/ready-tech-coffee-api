using Microsoft.Extensions.Caching.Memory;
using ReadyTech.CoffeeAPI.Domain.BrewCoffee;
using ReadyTech.CoffeeAPI.Domain.OpenWeatherMap;
using ReadyTech.CoffeeAPI.Infrastructure.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpenWeatherMapOptions>(builder.Configuration.GetSection("OpenWeatherMap"));

builder.Services.AddHttpClient<OpenWeatherMapClient>();
builder.Services.AddTransient<IGetBrewCoffeeHandler, GetBrewCoffeeHandler>();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapGet("/brew-coffee", async (IGetBrewCoffeeHandler getBrewCoffeeHandler) =>
    { 
        var response = await getBrewCoffeeHandler.HandleAsync();
        return Results.Ok(response);
    })
    .AddEndpointFilter(async (endpointFilterInvocationContext, next) =>
    {
        var dateTimeProvider = app.Services.GetRequiredService<IDateTimeProvider>();
        bool isTeapot() => dateTimeProvider.Now.Month == 4 && dateTimeProvider.Now.Day == 1;

        if (isTeapot())
        {
            return Results.StatusCode(418);
        }

        return await next(endpointFilterInvocationContext);
    })
    .AddEndpointFilter(async (endpointFilterInvocationContext, next) =>
    {
        var memoryCache = app.Services.GetRequiredService<IMemoryCache>();

        bool IsServiceUnavailable()
        {
            const string CallCounterKey = "CallCounter";
            var callCounter = memoryCache.GetOrCreate(CallCounterKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return 0;
            });

            callCounter++;
            memoryCache.Set(CallCounterKey, callCounter);

            return callCounter % 5 == 0;
        }

        if (IsServiceUnavailable())
        {
            return Results.StatusCode(503);
        }

        return await next(endpointFilterInvocationContext);
    });

app.Run();

public partial class Program { }