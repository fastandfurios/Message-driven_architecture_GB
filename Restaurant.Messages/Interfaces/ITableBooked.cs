namespace Restaurant.Messages.Interfaces
{
    public interface ITableBooked
    {
        Guid OrderId { get; }
        bool Success { get; }
    }
}
