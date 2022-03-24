using MassTransit;
using Restaurant.Messaging;

namespace Restaurant.Kitchen.Consumers
{
    public class TableBookedConsumer : IConsumer<ITableBooked>
    {
        private readonly Manager _manager;

        public TableBookedConsumer(Manager manager)
        {
            _manager = manager;
        }

        public Task Consume(ConsumeContext<ITableBooked> context)
        {
            var result = context.Message.Success;

            if (result)
            {
                _manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder);
            }

            return context.ConsumeCompleted;
        }
    }
}
