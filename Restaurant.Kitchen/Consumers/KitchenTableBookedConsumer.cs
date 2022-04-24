using System.Data.SQLite;
using System.Diagnostics;
using MassTransit;
using Restaurant.Kitchen.DAL.Models;
using Restaurant.Kitchen.DAL.Repositories.Interfaces;
using Restaurant.Kitchen.Exceptions;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Kitchen.Consumers
{
    public class KitchenTableBookedConsumer : IConsumer<IBookingRequest>
    {
        private readonly Manager _manager;
        private readonly IKitchenMessageRepository<KitchenTableBookedModel> _repository;

        public KitchenTableBookedConsumer(Manager manager, IKitchenMessageRepository<KitchenTableBookedModel> repository)
        {
            _manager = manager;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            try
            {
                _repository.Add(new KitchenTableBookedModel
                {
                    MessageId = context.MessageId!.Value,
                    OrderId = context.Message.OrderId
                });

                var random = new Random().Next(1000, 10000);

                Console.WriteLine($"[Заказ {context.Message.OrderId}] Проверка на кухне займет: {random}");
                await Task.Delay(random);

                var (confirmation, dish) =
                    _manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder!);
                if (confirmation)
                {
                    await context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId, true));
                }
                else
                {
                    if (dish.Name.Equals(Dishes.Lasagna.ToString()))
                        throw new LasagnaException(
                            $"Был принят предзаказ [{context.Message.OrderId}] с {Dishes.Lasagna}");

                    await context.Publish<IKitchenAccident>(new KitchenAccident(context.Message.OrderId, dish!));
                }
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine(e);
                await context.ConsumeCompleted;
            }
        }
    }
}
