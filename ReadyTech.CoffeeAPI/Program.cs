using ReadyTech.CoffeeAPI.Domain.BrewCoffee;
using ReadyTech.CoffeeAPI.Infrastructure;

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

builder.Services.AddTransient<IGetBrewCoffeeResponseBuilder, GetBrewCoffeeResponseBuilder>();
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
});

app.Run();

public partial class Program { }