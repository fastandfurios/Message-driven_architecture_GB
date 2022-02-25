namespace Restaurant
{
    public class Notification
    {
        private readonly ICollection<string> _cache = new List<string>();
        private const string _fileName = "notifications.txt";

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

        public async Task NotifyAsync(string key, int id = default, CancellationToken token = default)
        {
            await Task.Run(async () =>
                {
                    var entry = _cache.FirstOrDefault(n => n.Contains(key))!
                        .Split(":", StringSplitOptions.RemoveEmptyEntries)[1];

                    await Task.Delay(1000, token).ConfigureAwait(true);

                    Console.ForegroundColor = key is "[WARNING]" ? ConsoleColor.DarkYellow : ConsoleColor.DarkGreen;
                    Console.WriteLine(id == 0 ? entry : $"{entry} {id}");
                    Console.ResetColor();
                }, token);
        }
    }
}
