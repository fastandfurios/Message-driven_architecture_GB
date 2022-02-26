using System.Text.RegularExpressions;

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

        public void NotifyAsync(NKeys nKey, string subEntry = "", int id = default, bool isAwait = true, CancellationToken token = default)
        {
            Task.Run(async () =>
            {
                if(isAwait)
                    await Task.Delay(1000 * 2, token).ConfigureAwait(true);

                var entry = _cache.FirstOrDefault(n => n.Contains(nKey.ToString()))!
                    .Split(":", StringSplitOptions.RemoveEmptyEntries)[1];

                Console.ForegroundColor = nKey switch
                {
                    NKeys.NOT_1 => ConsoleColor.DarkGreen,
                    NKeys.NOT_2 => ConsoleColor.DarkGreen,
                    NKeys.NOT_3 => ConsoleColor.DarkGreen,
                    NKeys.NOT_4 => ConsoleColor.DarkGreen,
                    NKeys.WARNING => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.White
                };
                Console.WriteLine(id == 0 ? $"{subEntry}{entry.Replace("\\n", "\n")}" : $"{subEntry}{entry} {id}");
                Console.ResetColor();
            }, token);
        }
    }
}
