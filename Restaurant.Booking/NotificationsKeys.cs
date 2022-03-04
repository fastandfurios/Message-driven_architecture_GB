namespace Restaurant.Booking
{
    /// <summary> Перечисление ключевых фраз уведомлений </summary>
    [Flags]
    public enum NotificationsKeys
    {
        NotificationCancelBookingLine,
        NotificationCancelBooking,
        Introduction_1,
        Introduction_2,
        NotificationGoodbye,
        NotificationMessage_1,
        NotificationMessage_2,
        NotificationMessage_3,
        NotificationMessage_4,
        NotificationBookingLine,
        NotificationBooking,
        NotificationWarning,
    }
}
