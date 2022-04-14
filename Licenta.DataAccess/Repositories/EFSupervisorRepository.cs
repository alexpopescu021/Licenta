using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.DataAccess.Repositories
{
    public class EFSupervisorRepository:EFBaseRepository<Supervisor> , ISupervisorRepository
    {
        public EFSupervisorRepository(ApplicationDbContext context):base(context)
        {

        }

        public Supervisor GetByUserId(string userId)
        {
            return dbContext.Supervisors.Where(o => o.UserId == userId).FirstOrDefault();
        }
    }
}
