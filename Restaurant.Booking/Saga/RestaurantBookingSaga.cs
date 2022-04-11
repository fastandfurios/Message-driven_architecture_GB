using MassTransit;
using Restaurant.Booking.Consumers;
using Restaurant.Messages.Implementation;
using Restaurant.Messages.Interfaces;

#nullable disable
namespace Restaurant.Booking.Saga
{
    public class RestaurantBookingSaga : MassTransitStateMachine<RestaurantBooking>
    {
        #region constructor
        public RestaurantBookingSaga()
        {
            #region config
            InstanceState(prop => prop.CurrentState);

            Event(() => BookingRequested,
                cfg => cfg.CorrelateById(context => context.Message.OrderId)
                    .SelectId(context => context.Message.OrderId));

            Event(() => TableBooked,
                cfg => cfg.CorrelateById(context => context.Message.OrderId));

            Event(() => KitchenReady,
                cfg => cfg.CorrelateById(context => context.Message.OrderId));

            Event(() => KitchenAccident,
                cfg => cfg.CorrelateById(context => context.Message.OrderId));

            CompositeEvent(() => BookingApproved,
            tracking => tracking.ReadyEventStatus, KitchenReady, TableBooked);

            Event(() => BookingRequestFault,
                cfg => cfg.CorrelateById(prop => prop.Message.Message.OrderId));

            Event(() => NotificationFault,
                cfg => cfg.CorrelateById(prop => prop.Message.Message.OrderId));

            Schedule(() => BookingExpired,
                token => token.ExpirationId, cfg =>
                {
                    cfg.Delay = TimeSpan.FromSeconds(100);
                    cfg.Received = e => e.CorrelateById(context => context.Message.OrderId);
                });

            Schedule(() => GuestWaitingTimeExpired,
                token => token.WaitingId, cfg =>
                {
                    cfg.Received = e => e.CorrelateById(context => context.Message.OrderId);
                });

            Schedule(() => TimeArrivalGuestExpired,
                token => token.ArrivalId, cfg =>
                {
                    cfg.Received = e => e.CorrelateById(context => context.Message.OrderId);
                });
            #endregion

            #region build
            Initially(
                When(BookingRequested)
                    .Then(action =>
                    {
                        action.Saga.CorrelationId = action.Message.OrderId;
                        action.Saga.OrderId = action.Message.OrderId;
                        action.Saga.ClientId = action.Message.ClientId;
                        action.Saga.ArrivalTime = action.Message.ArrivalTime;
                    })
                    .Schedule(BookingExpired, factory => new BookingExpire(factory.Saga))
                    .TransitionTo(AwaitingBookingApproved)
            );

            During(AwaitingBookingApproved,
                When(BookingApproved)
                    .Unschedule(BookingExpired)
                    .Publish(factory => (INotify)new Notify(factory.Saga.ClientId,
                        factory.Saga.OrderId, "Стол успешно забронирован"))
                    .Then(action => Console.WriteLine($"Ожидание гостя {action.Saga.ClientId}"))
                    .Schedule(GuestWaitingTimeExpired, factory => new GuestWaitingTimeExpire(factory.Saga),
                        delay => delay.Delay = TimeSpan.FromSeconds(Random.Shared.Next(7, 15)))
                    .Schedule(TimeArrivalGuestExpired, factory => new TimeArrivalGuestExpire(factory.Saga),
                        delay => delay.Saga.ArrivalTime)
                    .TransitionTo(GuestWaitingTimeState),

                When(BookingRequestFault)
                    .Then(action => Console.WriteLine("Что-то пошло не так!"))
                    .Publish(factory => (INotify)new Notify(factory.Saga.ClientId,
                        factory.Saga.OrderId,
                        "Приносим извенения, стол забронировать не получилось."))
                    .Finalize(),

                When(NotificationFault)
                    .Then(action => Console.WriteLine("В сервисе уведомлений произошла ошибка. Уведомление не будет отправлено"))
                    .Finalize(),

                When(BookingExpired!.Received)
                    .Then(action => Console.WriteLine($"Отмена заказа {action.Saga.OrderId}"))
                    .Finalize(),

                When(KitchenAccident)
                    .Publish(factory => (INotify)new Notify(factory.Saga.ClientId,
                        factory.Saga.OrderId,
                        $"Отмена бронирования стола по заказу в связи с отсутсвием блюда {factory.Message.Dish.Name}!"))
                    .Finalize()
            );

            During(GuestWaitingTimeState,
                When(GuestWaitingTimeExpired!.Received)
                    .Unschedule(TimeArrivalGuestExpired)
                    .Then(action => Console.WriteLine($"Гость {action.Saga.ClientId} прибыл"))
                    .Finalize(),

                When(TimeArrivalGuestExpired!.Received)
                    .Unschedule(GuestWaitingTimeExpired)
                    .Then(action => Console.WriteLine($"Гость {action.Saga.ClientId} не пришел"))
                    .Finalize()
                );

            SetCompletedWhenFinalized();
            #endregion
        }
        #endregion

        #region properties
        public MassTransit.State AwaitingBookingApproved { get; private set; }
        public MassTransit.State GuestWaitingTimeState { get; private set; }
        public Event BookingApproved { get; private set; }
        public Schedule<RestaurantBooking, IBookingExpire> BookingExpired { get; private set; }
        public Event<IBookingRequest> BookingRequested { get; private set; }
        public Event<Fault<IBookingRequest>> BookingRequestFault { get; private set; }
        public Event<Fault<INotify>> NotificationFault { get; private set; }
        public Event<IKitchenReady> KitchenReady { get; private set; }
        public Event<ITableBooked> TableBooked { get; private set; }
        public Event<IKitchenAccident> KitchenAccident { get; private set; }
        public Schedule<RestaurantBooking, IGuestWaitingTimeExpire> GuestWaitingTimeExpired { get; private set; }
        public Schedule<RestaurantBooking, ITimeArrivalGuestExpire> TimeArrivalGuestExpired { get; private set; }
        #endregion
    }
}
