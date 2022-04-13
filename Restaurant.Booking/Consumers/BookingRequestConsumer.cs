using MassTransit;
using Restaurant.Booking.Models;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;
using Restaurant.Messages.Repositories.Interfaces;

#nullable disable
namespace Restaurant.Booking.Consumers
{
    public class BookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Restaurant _restaurant;
        private readonly IInMemoryRepository<BookingRequestModel> _repository;

        public BookingRequestConsumer(Restaurant restaurant, IInMemoryRepository<BookingRequestModel> repository)
        {
            _restaurant = restaurant;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            var model = _repository.Get().FirstOrDefault(model => model.OrderId == context.Message.OrderId);

            if (model is not null && model.CheckMessageId(context.MessageId.ToString()))
            {
                Console.WriteLine(context.MessageId.ToString());
                Console.WriteLine("Second time");
                return;
            }

            var requestModel = new BookingRequestModel(context.Message.OrderId,
                context.Message.ClientId,
                context.Message.PreOrder,
                context.Message.CreationDate,
                context.MessageId.ToString(),
                context.Message.ArrivalTime);

            Console.WriteLine(context.MessageId.ToString());
            Console.WriteLine("First time");
            var resultModel = model?.Update(requestModel, context.MessageId.ToString()) ?? requestModel;

            _repository.AddOrUpdate(resultModel);
            var result = await _restaurant.BookFreeTableAsync(1);
            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        }
    }
}
