using MassTransit;
using Microsoft.Extensions.Logging;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Booking.Consumers
{
    public class BookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        private readonly Restaurant _restaurant;
        private readonly ILogger<BookingRequestFaultConsumer> _logger;

        public BookingRequestFaultConsumer(Restaurant restaurant, ILogger<BookingRequestFaultConsumer> logger)
        {
            _restaurant = restaurant;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            _logger.Log(LogLevel.Warning, $"[OrderId {context.Message.Message.OrderId}] Отмена в зале");
            _restaurant.CancelReservationAsync();

            return context.ConsumeCompleted;
        }
    }
}
