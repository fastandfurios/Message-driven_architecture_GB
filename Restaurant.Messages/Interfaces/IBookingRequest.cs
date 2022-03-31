namespace Restaurant.Messages.Interfaces
{
    public interface IBookingRequest
    {
        Guid ClientId { get; }
        DateTime DateTime { get; }
        Guid OrderId { get; }
        Dish? PreOrder { get; }
    }
}
