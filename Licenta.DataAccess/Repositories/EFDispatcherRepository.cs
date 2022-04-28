using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    public class EFDispatcherRepository : EFBaseRepository<Dispatcher>, IDispatcherRepository
    {
        public EFDispatcherRepository(ApplicationDbContext context) : base(context)
        {

        }

        public Dispatcher GetByUserId(string userId)
        {
            return dbContext.Dispatchers.Where(o => o.UserId == userId).FirstOrDefault();
        }
    }
}
