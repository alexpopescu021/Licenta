using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    public class EfDispatcherRepository : EfBaseRepository<Dispatcher>, IDispatcherRepository
    {
        public EfDispatcherRepository(ApplicationDbContext context) : base(context)
        {

        }

        public Dispatcher GetByUserId(string userId)
        {
            return DbContext.Dispatchers.FirstOrDefault(o => o.UserId == userId);
        }
    }
}
