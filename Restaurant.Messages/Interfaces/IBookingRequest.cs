namespace Restaurant.Messages.Interfaces
{
    public interface IBookingRequest
    {
        Guid ClientId { get; }
        Guid OrderId { get; }
        Dish? PreOrder { get; }
    }
}
