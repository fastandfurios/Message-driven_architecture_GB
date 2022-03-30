namespace Restaurant.Messaging
{
    public interface IKitchenAccident
    {
        public Guid OrderId { get; }
        public Dish Dish { get; }
    }
}
