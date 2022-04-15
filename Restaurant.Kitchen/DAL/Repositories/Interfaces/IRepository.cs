namespace Restaurant.Kitchen.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        IEnumerable<T> Get();
    }
}
