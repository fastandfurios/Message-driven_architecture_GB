using MassTransit;

namespace Restaurant.Booking.Saga
{
    public class RestaurantBooking : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public int ReadyEventStatus { get; set; }
        public Guid? ExpirationId { get; set; }
        public Guid? WaitingId { get; set; }
        public Guid? ArrivalId { get; set; }
        public TimeSpan ArrivalTime { get; set; }
    }
}
