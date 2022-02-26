namespace Restaurant
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new();
        private readonly Notification _notification;
        private readonly object _lock = new();

        public Restaurant(Notification notification)
        {
            _notification = notification;

            for (ushort i = 1; i <= 10; i++)
                _tables.Add(new(i));

            new Timer(async _ => await CancelAutomaticallyAsync(), null, 20_000, 20_000);
        }

        private async Task CancelAutomaticallyAsync(CancellationToken token = default)
        {
            await Task.Run(() =>
            {
                var table = _tables.FirstOrDefault(t => t.State == State.Booked);

                if (table is not null)
                {
                    table.SetState(State.Free);
                    _notification.NotifyAsync(NKeys.NOT_4, "УВЕДОМЛЕНИЕ:", table.Id, token: token);
                }
                    
            }, token);
        }

        public void BookFreeTable(int countOfPersons)
        {
            _notification.NotifyAsync(NKeys.NOT_WEL, isAwait: false);

            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons 
                                                    && t.State == State.Free);

            Thread.Sleep(1000*5);
            table?.SetState(State.Booked);

            if (table is null)
                _notification.NotifyAsync(NKeys.NOT_1);
            else
                _notification.NotifyAsync(NKeys.NOT_2, id: table.Id);
        }

        public void CancelReservation(int id)
        {
            _notification.NotifyAsync(NKeys.CANCEL_WEL, isAwait: false);

            var table = _tables.FirstOrDefault(t => t.Id == id
                                                && t.State == State.Booked);
            Thread.Sleep(1000 * 5);
            table?.SetState(State.Free);

            if (table is null)
                _notification.NotifyAsync(NKeys.NOT_3);
            else
                _notification.NotifyAsync(NKeys.NOT_4, id: table.Id);
        }

        public void BookFreeTableAsync(int countOfPersons, CancellationToken token = default)
        {
            _notification.NotifyAsync(NKeys.NOT_WEL_ASYNC, isAwait: false, token: token);

            Task.Run(async () =>
            {
                Table? table;

                lock (_lock)
                {
                    
                    table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                            && t.State == State.Free);
                    table?.SetState(State.Booked);
                }

                await Task.Delay(1000 * 5, token).ConfigureAwait(true);

                if (table is null)
                    _notification.NotifyAsync(NKeys.NOT_1, "УВЕДОМЛЕНИЕ:", token: token);
                else
                    _notification.NotifyAsync(NKeys.NOT_2, "УВЕДОМЛЕНИЕ:", table.Id, token: token);
            }, token);
        }

        public void CancelReservationAsync(int id = default, CancellationToken token = default)
        {
            _notification.NotifyAsync(NKeys.CANCEL_WEL_ASYNC, isAwait: false, token: token);

            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.Id == id
                                                        && t.State == State.Booked);

                await Task.Delay(1000 * 5, token).ConfigureAwait(true);

                table?.SetState(State.Free);

                if (table is null)
                    _notification.NotifyAsync(NKeys.NOT_3, "УВЕДОМЛЕНИЕ:", token: token);
                else
                    _notification.NotifyAsync(NKeys.NOT_4, "УВЕДОМЛЕНИЕ:", table.Id, token: token);
            }, token);
        }
    }
}
