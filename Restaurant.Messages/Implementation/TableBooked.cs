using Restaurant.Messages.Interfaces;

namespace Restaurant.Messages.Implementation
{
    public class TableBooked : ITableBooked
    {
        public TableBooked(Guid orderId, bool success)
        {
            OrderId = orderId;
            Success = success;
        }
        
        public Guid OrderId { get; }
        public bool Success { get; }
    }
}
