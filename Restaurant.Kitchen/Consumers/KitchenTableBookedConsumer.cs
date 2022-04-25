using System.Data.SQLite;
using MassTransit;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<KitchenTableBookedConsumer> _logger;

        public KitchenTableBookedConsumer(Manager manager,
            IKitchenMessageRepository<KitchenTableBookedModel> repository,
            ILogger<KitchenTableBookedConsumer> logger)
        {
            _manager = manager;
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            _repository.Add(new KitchenTableBookedModel
            {
                MessageId = context.MessageId!.Value,
                OrderId = context.Message.OrderId
            });

            var random = new Random().Next(1000, 10000);

            _logger.Log(LogLevel.Information, $"[OrderId {context.Message.OrderId}]");
            Console.WriteLine($"Проверка на кухне займет: {random}");
            await Task.Delay(random);

            var (confirmation, dish) = _manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder!);

            if (dish.Name.Equals(Dishes.Lasagna.ToString()))
                throw new LasagnaException($"Был принят предзаказ [{context.Message.OrderId}] с {Dishes.Lasagna}");

            if (confirmation)
            {
                await context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId, true));
            }
            else
            {
                await context.Publish<IKitchenAccident>(new KitchenAccident(context.Message.OrderId, dish));
            }
        }
    }
}
