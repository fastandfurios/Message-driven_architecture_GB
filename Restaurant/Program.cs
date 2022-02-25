#region references
using System.Diagnostics;
using System.Text;
using Restaurant;
#endregion

Console.OutputEncoding = Encoding.UTF8;
var notification = new Notification();
await notification.ReadNotificationAsync().ConfigureAwait(true);
var rest = new Restaurant.Restaurant(notification);
while (true)
{
    var stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("Привет!");
    stringBuilder.AppendLine("1 - Желаете забронировать столик?");
    stringBuilder.Append("2 - Или отменить бронирование?");
    Console.WriteLine(stringBuilder);

    if (!int.TryParse(Console.ReadLine(), out var variant) || variant is not (1 or 2))
    {
        await notification.NotifyAsync("[WARNING]");
        continue;
    }

    Console.WriteLine("1 - мы уведомим Вас по смс (асинхронно)\n" +
                      "2 - подождите на линии, мы Вас оповестим (синхронно)");

    if (!int.TryParse(Console.ReadLine(), out var choice) || choice is not (1 or 2))
    {
        await notification.NotifyAsync("[WARNING]");
        continue;
    }

    var request = (variant, choice);

    var stopWatch = new Stopwatch();
    stopWatch.Start();

    switch (request)
    {
        case (1, 1):
            await rest.BookFreeTableAsync(1);
            break;
        case (1, 2):
            rest.BookFreeTable(1);
            break;
        case (2, 1):
            await rest.CancelReservationAsync(1);
            break;
        case (2, 2):
            rest.CancelReservation(1);
            break;
    }

    Console.WriteLine("Спасибо за Ваше обращение!");
    stopWatch.Stop();
    var ts = stopWatch.Elapsed;
    Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
}