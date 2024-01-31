using ReadyTech.CoffeeAPI.Domain.BrewCoffee;
using ReadyTech.CoffeeAPI.Domain.OpenWeatherMap;
using ReadyTech.CoffeeAPI.Infrastructure.Middleware;
using ReadyTech.CoffeeAPI.Infrastructure.Providers;
using ReadyTech.CoffeeAPI.Infrastructure.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new Iso8601DateTimeConverter());
                });

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

app.UseBrewCoffeeMiddleware();

app.UseEndpoints(endpoints => 
{
    _ = endpoints.MapBrewCoffeeEndpoint("/brew-coffee");
    _ = endpoints.MapControllers();
});

app.Run();

public partial class Program { }