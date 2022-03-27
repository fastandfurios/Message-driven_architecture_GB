using MassTransit;
using Restaurant.Messaging;

namespace Restaurant.Booking
{
    public class Manager
    {
        private readonly IBus _bus;

        public Manager(IBus bus)
        {
            _bus = bus;
        }

        public void SendNotification(Guid orderId, Dish? dish)
        {
            _bus.Publish(new CancellationBooking(orderId, dish));
        }
    }
}
