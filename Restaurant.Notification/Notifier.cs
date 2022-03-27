using System.Collections.Concurrent;
using Restaurant.Messaging;

namespace Restaurant.Notification
{
    [Flags]
    public enum Accepted
    {
        Rejected = 0,
        Kitchen = 1,
        Booking = 2,
        All = Kitchen | Booking,
    }

    public class Notifier
    {
        private readonly ConcurrentDictionary<Guid, (Guid?, Accepted)> _state = new();

        public void Accept(Guid orderId, Accepted accepted, Guid? clientId = null)
        {
            _state.AddOrUpdate(orderId, (clientId, accepted),
                (guid, oldValue) => 
                    (oldValue.Item1 ?? clientId, oldValue.Item2 | accepted));

            Notify(orderId);
        }

        private void Notify(Guid orderId)
        {
            var booking = _state[orderId];

            switch (booking.Item2)
            {
                case Accepted.All:
                    Console.WriteLine($"Успешно забронировано для клиента {booking.Item1}");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.Rejected:
                    Console.WriteLine($"Гость {booking.Item1}, к сожалению, все столики заняты");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.Kitchen:
                case Accepted.Booking:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Notify(Guid orderId, Dish? dish)
        {
            Console.WriteLine($"Отмена бронирования стола по заказу {orderId} в связи с отсутсвием блюда {dish.Name}!");
        }
    }
}
