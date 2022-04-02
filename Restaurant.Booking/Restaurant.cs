namespace Restaurant.Booking
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new();
        private readonly object _lock = new();

        public Restaurant()
        {
            for (ushort i = 1; i <= 10; i++)
                _tables.Add(new(i));
        }

        public async Task<bool?> BookFreeTableAsync(int countOfPersons, CancellationToken token = default)
        {
            Console.WriteLine(
                "Спасибо за Ваше обращение, я подберу столик и подтвержу вашу бронь, Вам придет уведомление");

            Table? table;

            lock (_lock)
            {
                table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                    && t.State == State.Free);
                table?.SetState(State.Booked);
            }

            await Task.Delay(100, token).ConfigureAwait(true);

            return !(table is null);
        }

        public void CancelReservationAsync(int id = default, CancellationToken token = default)
        {
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.Id == id
                                                        && t.State == State.Booked);

                await Task.Delay(1000 * 5, token).ConfigureAwait(true);

                table?.SetState(State.Free);

            }, token);
        }
    }
}
