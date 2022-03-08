#region references
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;
using RestaurantProject.Booking;
#endregion

Console.OutputEncoding = Encoding.UTF8;
var rest = new Restaurant();

while (true)
{
    Console.WriteLine("Привет!\n1 - Желаете забронировать столик?\n2 - Или отменить бронирование?");

    if (!int.TryParse(Console.ReadLine(), out var variant) || variant is not (1 or 2))
    {
        Console.WriteLine("Введите, пожалуйста 1 или 2");
        continue;
    }

    Console.WriteLine("1 - мы уведомим Вас по смс (асинхронно)\n2 - подождите на линии, мы Вас оповестим (синхронно)");

    if (!int.TryParse(Console.ReadLine(), out var choice) || choice is not (1 or 2))
    {
        Console.WriteLine("Введите, пожалуйста 1 или 2");
        continue;
    }

    var request = (variant, choice);

    var stopWatch = new Stopwatch();
    stopWatch.Start();

    switch (request)
    {
        case ((int)Choice.FirstChoice, (int)Choice.FirstChoice):
            rest.BookFreeTableAsync(1);
            break;
        case ((int)Choice.FirstChoice, (int)Choice.SecondChoice):
            rest.BookFreeTable(1);
            break;
        case ((int)Choice.SecondChoice, (int)Choice.FirstChoice):
            rest.CancelReservationAsync(1);
            break;
        case ((int)Choice.SecondChoice, (int)Choice.SecondChoice):
            rest.CancelReservation(1);
            break;
    }

    Console.WriteLine("Спасибо за Ваше обращение!");
    stopWatch.Stop();
    var ts = stopWatch.Elapsed;
    Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
}

internal enum Choice
{
    /// <summary> 1 вариант выбора </summary>
    FirstChoice = 1,
    /// <summary> 2 вариант выбора </summary>
    SecondChoice = 2
}