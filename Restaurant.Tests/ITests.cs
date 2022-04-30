using System.Threading.Tasks;

namespace Restaurant.Tests
{
    public interface ITests
    {
        Task Init();
        Task TearDown();
    }
}
