using Licenta.Model;

namespace Licenta.DataAccess.Abstractions
{
    public interface IDispatcherRepository : IBaseRepository<Dispatcher>
    {
        Dispatcher GetByUserId(string userId);
    }
}
