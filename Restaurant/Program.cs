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
    notification.NotifyAsync(NKeys.CASE_1, isAwait: false);

    if (!int.TryParse(Console.ReadLine(), out var variant) || variant is not (1 or 2))
    {
        notification.NotifyAsync(NKeys.WARNING, isAwait: false);
        continue;
    }

    notification.NotifyAsync(NKeys.CASE_2, isAwait: false);

    if (!int.TryParse(Console.ReadLine(), out var choice) || choice is not (1 or 2))
    {
        notification.NotifyAsync(NKeys.WARNING, isAwait: false);
        continue;
    }

    var request = (variant, choice);

    var stopWatch = new Stopwatch();
    stopWatch.Start();

    switch (request)
    {
        case (1, 1):
            rest.BookFreeTableAsync(5);
            break;
        case (1, 2):
            rest.BookFreeTable(5);
            break;
        case (2, 1):
            rest.CancelReservationAsync(1);
            break;
        case (2, 2):
            rest.CancelReservation(1);
            break;
    }

    notification.NotifyAsync(NKeys.END, isAwait: false);
    stopWatch.Stop();
    var ts = stopWatch.Elapsed;
    Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
}