using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Logging;
using Restaurant.Kitchen.DAL.Models;
using Restaurant.Kitchen.DAL.Repositories.Interfaces;

namespace Restaurant.Kitchen.DAL.Repositories.Implementation
{
    public class KitchenMessageRepository : IKitchenMessageRepository<KitchenTableBookedModel>
    {
        private readonly IConnection _connection;
        private readonly ILogger<KitchenMessageRepository> _logger;

        public KitchenMessageRepository(IConnection connection, ILogger<KitchenMessageRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public void Add(KitchenTableBookedModel entity)
        {
            try
            {
                using var connection = _connection.GetOpenedConnection();

                connection.Execute("INSERT INTO KitchenMessages(MessageId, OrderId) VALUES(@MessageId, @OrderId)",
                    new
                    {
                        MessageId = entity.MessageId,
                        OrderId = entity.OrderId
                    });
            }
            catch (SQLiteException e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                throw;
            }
        }

        public IEnumerable<KitchenTableBookedModel> Get()
        {
            try
            {
                using var connection = _connection.GetOpenedConnection();

                return connection.Query<KitchenTableBookedModel>("SELECT * FROM KitchenMessages");
            }
            catch (SQLiteException e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                throw;
            }
        }
    }
}
