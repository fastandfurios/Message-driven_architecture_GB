namespace Restaurant.Messages.Interfaces
{
    public interface INotify
    {
        Guid ClientId { get; }
        Guid OrderId { get; }
        string Message { get; }
    }
}
