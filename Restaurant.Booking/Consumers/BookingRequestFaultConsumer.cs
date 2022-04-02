using MassTransit;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Booking.Consumers
{
    public class BookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        private readonly Restaurant _restaurant;

        public BookingRequestFaultConsumer(Restaurant restaurant)
        {
            _restaurant = restaurant;
        }

        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            Console.WriteLine($"[OrderId {context.Message.Message.OrderId}] Отмена в зале");
            _restaurant.CancelReservationAsync();

            return context.ConsumeCompleted;
        }
    }
}
