using MassTransit;
using Restaurant.Kitchen.Exceptions;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Kitchen.Consumers
{
    public class KitchenTableBookedConsumer : IConsumer<IBookingRequest>
    {
        private readonly Manager _manager;

        public KitchenTableBookedConsumer(Manager manager)
        {
            _manager = manager;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            var random = new Random().Next(1000, 10000);

            Console.WriteLine($"[Заказ {context.Message.OrderId}] Проверка на кухне займет: {random}");
            await Task.Delay(random);
            
            var (confirmation, dish) = _manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder!);
            if (confirmation)
            {
                await context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId, true));
            }
            else
            {
                if(dish.Name.Equals(Dishes.Lasagna.ToString()))
                    throw new LasagnaException($"Был принят предзаказ [{context.Message.OrderId}] с {Dishes.Lasagna}");

                await context.Publish<IKitchenAccident>(new KitchenAccident(context.Message.OrderId, dish!));
            }
        }
    }
}
