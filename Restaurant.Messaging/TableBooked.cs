namespace Restaurant.Messaging
{
    public class TableBooked : ITableBooked
    {
        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public Dish? PreOrder { get; }
        public bool Success { get; set; }

        public TableBooked(Guid clientId, bool success, Dish? preOrder = null)
        {
            OrderId = Guid.NewGuid();
            ClientId = clientId;
            PreOrder = preOrder;
            Success = success;
        }
    }
}
