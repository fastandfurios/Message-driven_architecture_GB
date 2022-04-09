using MassTransit;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Kitchen.Consumers
{
    public class KitchenFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            Console.WriteLine($"Отмена приготовления заказа {context.Message.Message.OrderId} на кухне");
            
            return context.ConsumeCompleted;
        }
    }
}
