namespace ReadyTech.CoffeeAPI.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
