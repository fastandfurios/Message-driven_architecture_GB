using System.Data.SQLite;

namespace Restaurant.Kitchen.DAL.Repositories.Interfaces
{
    public interface IConnection
    {
        SQLiteConnection GetOpenedConnection();
    }
}
