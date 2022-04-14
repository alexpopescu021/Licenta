using System;
using System.Collections.Generic;
using System.Text;
using Licenta.Model;

namespace Licenta.DataAccess.Abstractions
{
    public interface ISupervisorRepository:IBaseRepository<Supervisor>
    {
        Supervisor GetByUserId(string userId);
    }
}
