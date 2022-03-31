using MassTransit;
using Restaurant.Booking.Consumers;
using Restaurant.Messages;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Booking.Saga
{
    public class RestaurantBookingSaga : MassTransitStateMachine<RestaurantBooking>
    {
        #region constructor
        public RestaurantBookingSaga()
        {
            InstanceState(prop => prop.CurrentState);

            Event(() => BookingRequested,
                cfg => cfg.CorrelateById(context => context.Message.OrderId)
                    .SelectId(context => context.Message.OrderId));

            Event(() => TableBooked,
                cfg => cfg.CorrelateById(context => context.Message.OrderId));

            Event(() => KitchenReady,
                cfg => cfg.CorrelateById(context => context.Message.OrderId));

            CompositeEvent(() => BookingApproved,
            tracking => tracking.ReadyEventStatus, KitchenReady, TableBooked);

            Event(() => BookingRequestFault,
                cfg => cfg.CorrelateById(prop => prop.Message.Message.OrderId));

            Schedule(() => BookingExpired,
                token => token.ExpirationId, cfg =>
                {
                    cfg.Delay = TimeSpan.FromSeconds(5);
                    cfg.Received = e => e.CorrelateById(context => context.Message.OrderId);
                });

            Initially(
                When(BookingRequested)
                    .Then(action =>
                    {
                        action.Saga.CorrelationId = action.Message.OrderId;
                        action.Saga.OrderId = action.Message.OrderId;
                        action.Saga.ClientId = action.Message.ClientId;
                    })
                    .Schedule(BookingExpired, 
                        factory => new BookingExpire(factory.Saga),
                        provider => TimeSpan.FromSeconds(100))
                    .TransitionTo(AwaitingBookingApproved)
            );

            During(AwaitingBookingApproved,
                When(BookingApproved)
                    .Unschedule(BookingExpired)
                    .Publish(factory => new Notify(factory.Saga.ClientId,
                        factory.Saga.OrderId, "Стол успешно забронирован"))
                    .Finalize(),
                
                When(BookingRequestFault)
                    .Then(action => Console.WriteLine("Что-то пошло не так!"))
                    .Publish(factory => new Notify(factory.Saga.ClientId,
                        factory.Saga.OrderId,
                        "Приносим извенения, стол забронировать не получилось."))
                    .Publish(factory => new CancellationBooking(factory.Saga.OrderId,
                        factory.Saga.ClientId,
                        ""))
                    .Finalize());

            When(BookingExpired!.Received)
                .Then(action => Console.WriteLine($"Отмена заказа {action.Saga.OrderId}"))
                .Finalize();

            SetCompletedWhenFinalized();
        }
        #endregion

        #region properties
        public MassTransit.State AwaitingBookingApproved { get; private set; }
        public Event BookingApproved { get; private set; }
        public Schedule<RestaurantBooking, IBookingExpire> BookingExpired { get; private set; }
        public Event<IBookingRequest> BookingRequested { get; private set; }
        public Event<Fault<IBookingRequest>> BookingRequestFault { get; private set; }
        public Event<IKitchenReady> KitchenReady { get; private set; }
        public Event<ITableBooked> TableBooked { get; private set; }
        #endregion
    }
}
