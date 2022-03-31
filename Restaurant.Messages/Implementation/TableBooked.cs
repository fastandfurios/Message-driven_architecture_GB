namespace Restaurant.Messages.Implementation
{
    public class TableBooked : ITableBooked
    {
        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public Dish? PreOrder { get; }
        public bool Success { get; set; }

        public TableBooked(Guid clientId, bool success, Guid orderId, Dish? preOrder = null)
        {
            ClientId = clientId;
            PreOrder = preOrder;
            Success = success;
            OrderId = orderId;
        }
    }
}
