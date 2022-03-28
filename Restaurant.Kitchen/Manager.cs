using MassTransit;
using Restaurant.Messaging;

namespace Restaurant.Kitchen
{
    public class Manager
    {
        public void CheckKitchenReady(Guid orderId, Dish? dish, ConsumeContext context)
        {
            switch (dish.Id)
            {
                case (int)Dishes.Pizza:
                case (int)Dishes.Burger:
                case (int)Dishes.Rolls:
                    context.Publish<IKitchenReady>(new KitchenReady(orderId, true));
                    break;
                case (int)Dishes.Chicken:
                    dish.Name = Dishes.Potato.ToString();
                    context.Publish<IKitchenAccident>(new KitchenAccident(orderId, dish));
                    break;
                case (int)Dishes.Potato:
                    dish.Name = Dishes.Potato.ToString();
                    context.Publish<IKitchenAccident>(new KitchenAccident(orderId, dish));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
