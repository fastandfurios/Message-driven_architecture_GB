using System.Collections.Concurrent;
using Restaurant.Messages.Repositories.Interfaces;

namespace Restaurant.Messages.Repositories.Implementation
{
    public class InMemoryRepository<T> : IInMemoryRepository<T> where T : class
    {
        private readonly ConcurrentBag<T> _repository = new();

        public void AddOrUpdate(T entity)
        {
            _repository.Add(entity);
        }

        public IEnumerable<T> Get()
        {
            return _repository;
        }
    }
}
