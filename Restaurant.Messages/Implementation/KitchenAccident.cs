using Restaurant.Messages.Interfaces;

namespace Restaurant.Messages.Implementation
{
    public class KitchenAccident : IKitchenAccident
    {
        public KitchenAccident(Guid orderId, Dish dish, Guid clientId, string message)
        {
            OrderId = orderId;
            Dish = dish;
            ClientId = clientId;
            Message = message;
        }

        public string Message { get; }
        public Guid OrderId { get; }
        public Dish Dish { get; }
        public Guid ClientId { get; }
    }
}
