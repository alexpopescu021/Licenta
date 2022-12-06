using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.DataAccess.Repositories
{
    public class EfRecipientRepository : EfBaseRepository<Recipient>, IRecipientRepository
    {
        public EfRecipientRepository(ApplicationDbContext dbContext) : base(dbContext)
        { }



    }
}
