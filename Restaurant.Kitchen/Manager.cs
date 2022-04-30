using Restaurant.Messages;

namespace Restaurant.Kitchen
{
    public class Manager
    {
        public (bool confirmation, Dish dish) CheckKitchenReady(Guid orderId, Dish dish)
        {
            switch (dish.Id)
            {
                case (int)Dishes.Pizza:
                    dish.Name = Dishes.Pizza.ToString();
                    return (true, dish);
                case (int)Dishes.Burger:
                    dish.Name = Dishes.Burger.ToString();
                    return (true, dish);
                case (int)Dishes.Rolls:
                    dish.Name = Dishes.Rolls.ToString();
                    return (true, dish);
                case (int)Dishes.Chicken:
                    dish.Name = Dishes.Chicken.ToString();
                    return (false, dish);
                case (int)Dishes.Potato:
                    dish.Name = Dishes.Potato.ToString();
                    return (false, dish);
                case (int)Dishes.Lasagna:
                    dish.Name = Dishes.Lasagna.ToString();
                    return (false, dish);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
