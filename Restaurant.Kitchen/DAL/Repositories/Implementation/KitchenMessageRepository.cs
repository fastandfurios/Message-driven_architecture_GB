using Dapper;
using Restaurant.Kitchen.DAL.Models;
using Restaurant.Kitchen.DAL.Repositories.Interfaces;

namespace Restaurant.Kitchen.DAL.Repositories.Implementation
{
    public class KitchenMessageRepository : IKitchenMessageRepository<KitchenTableBookedModel>
    {
        private readonly IConnection _connection;

        public KitchenMessageRepository(IConnection connection)
        {
            _connection = connection;
        }

        public void Add(KitchenTableBookedModel entity)
        {
            using var connection = _connection.GetOpenedConnection();

            connection.Execute("INSERT INTO KitchenMessages(MessageId, OrderId) VALUES(@MessageId, @OrderId)",
                new
                {
                    MessageId = entity.MessageId,
                    OrderId = entity.OrderId
                });
        }

        public IEnumerable<KitchenTableBookedModel> Get()
        {
            throw new NotImplementedException();
        }
    }
}
