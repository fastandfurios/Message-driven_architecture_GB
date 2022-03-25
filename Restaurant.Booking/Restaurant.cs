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
                "Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, Вам придет уведомление");

            Table? table;

            lock (_lock)
            {
                table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                    && t.State == State.Free);
                table?.SetState(State.Booked);
            }

            await Task.Delay(1000 * 5, token).ConfigureAwait(true);

            return table is null;
        }
    }
}
