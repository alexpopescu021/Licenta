using System;
using System.Collections.Generic;
using System.Text;
using Licenta.Model;

namespace Licenta.DataAccess.Abstractions
{
    public interface IEmployeeRepository
    {
        void AddEmployee(string userId, string name, string email,string role);
        
    }
}
