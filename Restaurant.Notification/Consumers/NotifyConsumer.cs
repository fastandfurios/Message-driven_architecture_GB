

#nullable disable
using MassTransit;
using Microsoft.Extensions.Logging;
using Restaurant.Messages.Interfaces;
using Restaurant.Messages.Repositories.Interfaces;
using Restaurant.Notification.Models;
namespace Restaurant.Notification.Consumers
{
    public class NotifyConsumer : IConsumer<INotify>
    {
        private readonly Notifier _notifier;
        private readonly IInMemoryRepository<NotifyModel> _repository;
        private readonly ILogger<NotifyConsumer> _logger;

        public NotifyConsumer(Notifier notifier, IInMemoryRepository<NotifyModel> repository, ILogger<NotifyConsumer> logger)
        {
            _notifier = notifier;
            _repository = repository;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<INotify> context)
        {
            var model = _repository.Get().FirstOrDefault(i => i.OrderId == context.Message.OrderId);

            if (model is not null && model.CheckMessageId(context.MessageId.ToString()))
            {
                _logger.Log(LogLevel.Information, context.Message.ToString());
                _logger.Log(LogLevel.Information, "Second time");
                return context.ConsumeCompleted;
            }

            var requestModel = new NotifyModel(context.Message.OrderId,
                context.Message.ClientId,
                context.Message.Message,
                context.MessageId.ToString());
            
            _logger.Log(LogLevel.Information, context.MessageId.ToString());
            _logger.Log(LogLevel.Information, "First time");
            var resultModel = model?.Update(requestModel, context.Message.ToString()!) ?? requestModel;

            _repository.AddOrUpdate(resultModel);
            _ = new Timer(_ => _repository.Delete(), null, 30000, 0);

            _notifier.Notify(context.Message.OrderId, context.Message.ClientId, context.Message.Message);
            
            return context.ConsumeCompleted;
        }
    }
}
