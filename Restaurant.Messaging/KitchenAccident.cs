namespace Restaurant.Messaging
{
    public class KitchenAccident : IKitchenAccident
    {
        public KitchenAccident(Guid orderId, Dish dish)
        {
            OrderId = orderId;
            Dish = dish;
        }

        public Guid OrderId { get; }
        public Dish Dish { get; }
    }
}
