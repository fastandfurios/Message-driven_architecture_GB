using MassTransit;
using Restaurant.Messaging;

namespace Restaurant.Kitchen
{
    public class Manager
    {
        private readonly IBus _bus;

        public Manager(IBus bus)
        {
            _bus = bus;
        }

        public void CheckKitchenReady(Guid orderId, Dish? dish)
        {
            switch (dish.Id)
            {
                case (int)Dishes.Pizza:
                case (int)Dishes.Burger:
                case (int)Dishes.Rolls:
                    _bus.Publish<IKitchenReady>(new KitchenReady(orderId, true));
                    break;
                case (int)Dishes.Chicken:
                    dish.Name = Dishes.Potato.ToString();
                    _bus.Publish<IKitchenAccident>(new KitchenAccident(orderId, dish));
                    break;
                case (int)Dishes.Potato:
                    dish.Name = Dishes.Potato.ToString();
                    _bus.Publish<IKitchenAccident>(new KitchenAccident(orderId, dish));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
