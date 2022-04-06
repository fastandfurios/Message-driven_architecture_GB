namespace Restaurant.Messages.Interfaces
{
    public interface IKitchenAccident
    {
        Dish Dish { get; }
        Guid OrderId { get; }
    }
}
