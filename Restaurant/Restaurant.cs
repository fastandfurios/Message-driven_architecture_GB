namespace Restaurant
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new();
        private readonly Notification _notification;

        public Restaurant(Notification notification)
        {
            _notification = notification;

            for (ushort i = 1; i <= 10; i++)
                _tables.Add(new(i));
        }

        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, оставайтесь на линии");

            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons 
                                                    && t.State == State.Free);

            Thread.Sleep(1000*5);
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

        public async Task BookFreeTableAsync(int countOfPersons, CancellationToken token = default)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, Вам придет уведомление");

            await Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                        && t.State == State.Free);
                await Task.Delay(1000 * 5, token).ConfigureAwait(true);
                table?.SetState(State.Booked);

                if (table is null)
                    await _notification.NotifyAsync("[1]", 0, token);
                else
                    await _notification.NotifyAsync("[2]", table.Id, token);
                
            }, token);
        }

        public async Task CancelReservationAsync(int id, CancellationToken token = default)
        {
            Console.WriteLine("Добрый день! Вам придет уведомление");

            await Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.Id == id 
                                                        && t.State == State.Booked);
                await Task.Delay(1000 * 5, token).ConfigureAwait(true);
                table?.SetState(State.Free);

                if (table is null)
                    await _notification.NotifyAsync("[3]", 0, token);
                else
                    await _notification.NotifyAsync("[4]", table.Id, token);

            }, token);
        }
    }
}
