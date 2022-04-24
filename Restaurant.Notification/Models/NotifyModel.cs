namespace Restaurant.Notification.Models
{
    public class NotifyModel
    {
        public NotifyModel(Guid orderId, Guid clientId, string message, string messageId)
        {
            _messageIds.Add(messageId);

            OrderId = orderId;
            ClientId = clientId;
            Message = message;
        }

        public NotifyModel Update(NotifyModel model, string messageId)
        {
            _messageIds.Add(messageId);

            OrderId = model.OrderId;
            ClientId = model.ClientId;
            Message = model.Message;

            return this;
        }

        public bool CheckMessageId(string messageId)
        {
            return _messageIds.Contains(messageId);
        }

        private readonly List<string> _messageIds = new();
        public Guid OrderId { get; private set; }
        public Guid ClientId { get; private set; }
        public string Message { get; private set; }
    }
}
