namespace Restaurant
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new();

        public Restaurant()
        {
            for (ushort i = 1; i <= 10; i++)
                _tables.Add(new(i));
        }

        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, оставайтесь на линии");

            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons 
                                                    && t.State == State.Free);
            Thread.Sleep(1000*5);
            Console.WriteLine(table is null
                ? "К сожалению, сейчас все столики заняты"
                : $"Готово! Ваш столик номер {table.Id}");
        }

        public async Task BookFreeTableAsync(int countOfPersons, CancellationToken token = default)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, Вам придет уведомление");

            await Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                        && t.State == State.Free);
                await Task.Delay(1000 * 5, token);
                table?.SetState(State.Booked);

                Console.WriteLine(table is null
                    ? $"УВЕДОМЛЕНИЕ. К сожалению, сейчас все столики заняты"
                    : $"УВЕДОМЛЕНИЕ. Готово! Ваш столик номер {table.Id}");
            }, token).ConfigureAwait(true);
        }
    }
}
