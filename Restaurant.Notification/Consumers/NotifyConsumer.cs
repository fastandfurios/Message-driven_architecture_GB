using MassTransit;
using Restaurant.Messages.Interfaces;
using Restaurant.Messages.Repositories.Interfaces;
using Restaurant.Notification.Models;

#nullable disable
namespace Restaurant.Notification.Consumers
{
    public class NotifyConsumer : IConsumer<INotify>
    {
        private readonly Notifier _notifier;
        private readonly IInMemoryRepository<NotifyModel> _repository;

        public NotifyConsumer(Notifier notifier, IInMemoryRepository<NotifyModel> repository)
        {
            _notifier = notifier;
            _repository = repository;
        }

        public Task Consume(ConsumeContext<INotify> context)
        {
            var model = _repository.Get().FirstOrDefault(i => i.OrderId == context.Message.OrderId);

            if (model is not null && model.CheckMessageId(context.MessageId.ToString()))
            {
                Console.WriteLine(context.Message.ToString());
                Console.WriteLine("Second time");
                return context.ConsumeCompleted;
            }

            var requestModel = new NotifyModel(context.Message.OrderId,
                context.Message.ClientId,
                context.Message.Message,
                context.MessageId.ToString());

            Console.WriteLine(context.MessageId.ToString());
            Console.WriteLine("First time");
            var resultModel = model?.Update(requestModel, context.Message.ToString()!) ?? requestModel;

            _repository.AddOrUpdate(resultModel);
            _notifier.Notify(context.Message.OrderId, context.Message.ClientId, context.Message.Message);
            
            return context.ConsumeCompleted;
        }
    }
}
