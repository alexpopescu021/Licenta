using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.DataAccess.Repositories
{
    public class EFRecipientRepository : EFBaseRepository<Recipient>, IRecipientRepository
    {
        public EFRecipientRepository(ApplicationDbContext dbContext) : base(dbContext)
        { }



    }
}
