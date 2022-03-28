using Restaurant.Messaging;

namespace Restaurant.Kitchen
{
    public class Manager
    {
        public (bool confirmation, Dish? dish) CheckKitchenReady(Guid orderId, Dish? dish)
        {
            switch (dish.Id)
            {
                case (int)Dishes.Pizza:
                case (int)Dishes.Burger:
                case (int)Dishes.Rolls:
                    return (true, dish);
                case (int)Dishes.Chicken:
                    dish.Name = Dishes.Chicken.ToString();
                    return (false, dish);
                case (int)Dishes.Potato:
                    dish.Name = Dishes.Potato.ToString();
                    return (false, dish);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
