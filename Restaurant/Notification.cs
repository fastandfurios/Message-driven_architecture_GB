namespace RestaurantProject
{
    public class Notification
    {
        private readonly ICollection<string> _cache = new List<string>();
        private const string _fileName = "notifications.txt";

        /// <summary> Читает уведомления из файла-источника </summary>
        /// <param name="token">токен отмены</param>
        public async Task ReadNotificationAsync(CancellationToken token = default)
        {
            var fileInfo = new FileInfo(_fileName);

            using var stream = fileInfo.OpenText();
            var entry = string.Empty;
            while ((entry = await stream.ReadLineAsync().ConfigureAwait(true)) is not null)
            {
                _cache.Add(entry);
            }
        }

        /// <summary> Уведомить асинхронно в консоль </summary>
        /// <param name="notificationKey">перечисление ключей уведомлений</param>
        /// <param name="additionalEntry">дополнительная строка для уведомления</param>
        /// <param name="id">номер столика</param>
        /// <param name="isAwait">флаг для управления задержкой</param>
        /// <param name="token">токен для отмены</param>
        public void NotifyAsync(NotificationsKeys notificationKey, string additionalEntry = "", int id = default, bool isAwait = true, CancellationToken token = default)
        {
            Task.Run(async () =>
            {
                if(isAwait)
                    await Task.Delay(1000 * 2, token).ConfigureAwait(true);

                var entry = _cache.FirstOrDefault(n => n.Contains(notificationKey.ToString()))!
                    .Split(":", StringSplitOptions.RemoveEmptyEntries)[1];

                Console.ForegroundColor = notificationKey switch
                {
                    NotificationsKeys.NotificationMessage_1 => ConsoleColor.DarkGreen,
                    NotificationsKeys.NotificationMessage_2 => ConsoleColor.DarkGreen,
                    NotificationsKeys.NotificationMessage_3 => ConsoleColor.DarkGreen,
                    NotificationsKeys.NotificationMessage_4 => ConsoleColor.DarkGreen,
                    NotificationsKeys.NotificationWarning => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.White
                };
                Console.WriteLine(id == 0 ? $"{additionalEntry}{entry.Replace("\\n", "\n")}" : $"{additionalEntry}{entry} {id}");
                Console.ResetColor();
            }, token);
        }
    }
}
