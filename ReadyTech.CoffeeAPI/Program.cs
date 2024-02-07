using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using ReadyTech.CoffeeAPI.Domain.BrewCoffee;
using ReadyTech.CoffeeAPI.Domain.OpenWeatherMap;
using ReadyTech.CoffeeAPI.Infrastructure.Providers;
using ReadyTech.CoffeeAPI.Infrastructure.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpenWeatherMapOptions>(builder.Configuration.GetSection("OpenWeatherMap"));

builder.Services.AddHttpClient<OpenWeatherMapClient>();
builder.Services.AddTransient<IGetBrewCoffeeHandler, GetBrewCoffeeHandler>();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new Iso8601DateTimeConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapGet("/brew-test", () => "Hello world!");

app.MapGet("/brew-coffee", async (IGetBrewCoffeeHandler getBrewCoffeeHandler) =>
    { 
        var response = await getBrewCoffeeHandler.HandleAsync();
        return Results.Ok(response);
    })
    .AddEndpointFilter(async (endpointFilterInvocationContext, next) =>
    {
        bool isTeapot()
        {
            var dateTimeProvider = app.Services.GetRequiredService<IDateTimeProvider>();
            return dateTimeProvider.Now.Month == 4 && dateTimeProvider.Now.Day == 1;
        } 

        if (isTeapot())
        {
            return Results.StatusCode(418);
        }

        return await next(endpointFilterInvocationContext);
    })
    .AddEndpointFilter(async (endpointFilterInvocationContext, next) =>
    {
        bool isServiceUnavailable()
        {
            const string CallCounterKey = "CallCounter";
            var memoryCache = app.Services.GetRequiredService<IMemoryCache>();
            var callCounter = memoryCache.GetOrCreate(CallCounterKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return 0;
            });

            callCounter++;
            memoryCache.Set(CallCounterKey, callCounter);

            return callCounter % 5 == 0;
        }

        if (isServiceUnavailable())
        {
            return Results.StatusCode(503);
        }

        return await next(endpointFilterInvocationContext);
    });

app.Run();

public partial class Program { }