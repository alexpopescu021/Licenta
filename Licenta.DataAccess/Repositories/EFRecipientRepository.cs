using System;
using System.Collections.Generic;
using System.Text;
using Licenta.DataAccess;
using Licenta.DataAccess.Abstractions;
using Licenta.DataAccess.Repositories;
using Licenta.Model;

namespace Licenta.DataAccess.Repositories
{
    public class EFRecipientRepository : EFBaseRepository<Recipient>, IRecipientRepository
    {
        public EFRecipientRepository(ApplicationDbContext dbContext) : base(dbContext)
        { }
    


    }
}
