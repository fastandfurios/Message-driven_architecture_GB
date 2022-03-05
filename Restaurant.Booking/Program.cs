#region references
using System.Diagnostics;
using System.Text;
using RestaurantProject.Booking;
#endregion

Console.OutputEncoding = Encoding.UTF8;
var notification = new Notification();
await notification.ReadNotificationAsync().ConfigureAwait(true);
var rest = new Restaurant(notification);

while (true)
{
    notification.NotifyAsync(NotificationsKeys.Introduction_1, isAwait: false);

    if (!int.TryParse(Console.ReadLine(), out var variant) || variant is not (1 or 2))
    {
        notification.NotifyAsync(NotificationsKeys.NotificationWarning, isAwait: false);
        continue;
    }

    notification.NotifyAsync(NotificationsKeys.Introduction_2, isAwait: false);

    if (!int.TryParse(Console.ReadLine(), out var choice) || choice is not (1 or 2))
    {
        notification.NotifyAsync(NotificationsKeys.NotificationWarning, isAwait: false);
        continue;
    }

    var request = (variant, choice);

    var stopWatch = new Stopwatch();
    stopWatch.Start();

    switch (request)
    {
        case ((int)Choice.first, (int)Choice.first):
            rest.BookFreeTableAsync(1);
            break;
        case ((int)Choice.first, (int)Choice.second):
            rest.BookFreeTable(1);
            break;
        case ((int)Choice.second, (int)Choice.first):
            rest.CancelReservationAsync(1);
            break;
        case ((int)Choice.second, (int)Choice.second):
            rest.CancelReservation(1);
            break;
    }

    notification.NotifyAsync(NotificationsKeys.NotificationGoodbye, isAwait: false);
    stopWatch.Stop();
    var ts = stopWatch.Elapsed;
    Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
}

internal enum Choice
{
    first = 1,
    second = 2
}