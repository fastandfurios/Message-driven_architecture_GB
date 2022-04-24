using System.Data.SQLite;
using Microsoft.Extensions.Configuration;
using Restaurant.Kitchen.DAL.Repositories.Interfaces;

namespace Restaurant.Kitchen.DAL.Repositories.Implementation
{
    public class Connection : IConnection
    {
        private const string _connectionString = "Data Source=C:\\Test\\Messages.db;Version=3;New=True;Compress=True;";

        public SQLiteConnection GetOpenedConnection() => new(_connectionString);
    }
}
