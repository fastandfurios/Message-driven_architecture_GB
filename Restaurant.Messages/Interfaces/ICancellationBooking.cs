namespace Restaurant.Messages.Interfaces
{
    public interface ICancellationBooking
    {
        Guid ClientId { get; }
        string Message { get; }
        Guid OrderId { get; }
        
    }
}
