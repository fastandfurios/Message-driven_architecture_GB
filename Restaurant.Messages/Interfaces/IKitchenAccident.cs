namespace Restaurant.Messages.Interfaces
{
    public interface IKitchenAccident
    {
        Dish Dish { get; }
        Guid ClientId { get; }
        string Message { get; }
        Guid OrderId { get; }
    }
}
