using MassTransit;
using Restaurant.Messaging;

namespace Restaurant.Kitchen.Consumers
{
    public class KitchenTableBookedConsumer : IConsumer<ITableBooked>
    {
        private readonly Manager _manager;

        public KitchenTableBookedConsumer(Manager manager)
        {
            _manager = manager;
        }

        public Task Consume(ConsumeContext<ITableBooked> context)
        {
            var result = context.Message.Success;

            if (result)
            {
                (bool confirmation, Dish? dish) conclusion = _manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder);
                if (conclusion.confirmation)
                {
                    context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId, true));
                }
                else
                {
                    context.Publish<IKitchenAccident>(new KitchenAccident(context.Message.OrderId, conclusion.dish!));
                }
            }

            return context.ConsumeCompleted;
        }
    }
}
