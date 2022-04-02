namespace Restaurant.Messages.Interfaces
{
    public interface IKitchenReady
    {
        Guid OrderId { get; }
        bool Ready { get; set; }
    }
}
