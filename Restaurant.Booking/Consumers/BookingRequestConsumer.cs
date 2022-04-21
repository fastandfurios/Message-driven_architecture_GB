#nullable disable
using MassTransit;
using Microsoft.Extensions.Logging;
using Restaurant.Booking.Models;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;
using Restaurant.Messages.Repositories.Interfaces;

namespace Restaurant.Booking.Consumers
{
    public class BookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Restaurant _restaurant;
        private readonly IInMemoryRepository<BookingRequestModel> _repository;
        private readonly ILogger<BookingRequestConsumer> _logger;

        public BookingRequestConsumer(Restaurant restaurant, IInMemoryRepository<BookingRequestModel> repository, ILogger<BookingRequestConsumer> logger)
        {
            _restaurant = restaurant;
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            var model = _repository.Get().FirstOrDefault(model => model.OrderId == context.Message.OrderId);

            if (model is not null && model.CheckMessageId(context.MessageId.ToString()))
            {
                _logger.Log(LogLevel.Information, context.MessageId.ToString());
                _logger.Log(LogLevel.Information, "Second time");
                return;
            }

            var requestModel = new BookingRequestModel(context.Message.OrderId,
                context.Message.ClientId,
                context.Message.PreOrder,
                context.Message.CreationDate,
                context.MessageId.ToString(),
                context.Message.ArrivalTime);

            _logger.Log(LogLevel.Information, context.MessageId.ToString());
            _logger.Log(LogLevel.Information, "First time");
            var resultModel = model?.Update(requestModel, context.MessageId.ToString()) ?? requestModel;

            _repository.AddOrUpdate(resultModel);
            _ = new Timer(_ => _repository.Delete(), null, 30000, 0);

            var result = await _restaurant.BookFreeTableAsync(1);
            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        }
    }
}
