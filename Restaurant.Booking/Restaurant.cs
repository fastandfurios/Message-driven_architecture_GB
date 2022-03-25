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

            //new Timer(async _ => await CancelAutomaticallyAsync(), null, 20_000, 20_000);
        }

        private async Task CancelAutomaticallyAsync(CancellationToken token = default)
        {
            await Task.Run(() =>
            {
                var table = _tables.FirstOrDefault(t => t.State == State.Booked);

                if (table is not null)
                {
                    table.SetState(State.Free);
                    Console.WriteLine($"Готово! Бронь снята со стола под номером {table.Id}");
                }

            }, token);
        }

        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, оставайтесь на линии");

            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                    && t.State == State.Free);

            Thread.Sleep(1000 * 5);
            table?.SetState(State.Booked);

            Console.WriteLine(table is null
                ? "К сожалению, сейчас все столики заняты"
                : $"Готово! Ваш столик номер {table.Id}");
        }

        public void CancelReservation(int id)
        {
            Console.WriteLine("Добрый день! Оставайтесь на линии");

            var table = _tables.FirstOrDefault(t => t.Id == id
                                                && t.State == State.Booked);
            Thread.Sleep(1000 * 5);
            table?.SetState(State.Free);

            Console.WriteLine(table is null
                ? "К сожалению, такого столика нет! Вы ошиблись с номером или он не был забронирован!"
                : $"Готово! Бронь снята со стола под номером {table.Id}");
        }

        public async Task<bool?> BookFreeTableAsync(int countOfPersons, CancellationToken token = default)
        {
            //Console.WriteLine(
            //    "Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, Вам придет уведомление");

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

        public void CancelReservationAsync(int id = default, CancellationToken token = default)
        {
            Console.WriteLine("Добрый день! Вам придет уведомление");

            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.Id == id
                                                        && t.State == State.Booked);

                await Task.Delay(1000 * 5, token).ConfigureAwait(true);

                table?.SetState(State.Free);

                Console.WriteLine(table is null
                    ? "УВЕДОМЛЕНИЕ: К сожалению, такого столика нет! Вы ошиблись с номером или он не был забронирован!"
                    : $"УВЕДОМЛЕНИЕ: Готово! Бронь снята со стола под номером {table.Id}");
            }, token);
        }
    }
}
