using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Licenta.DataAccess.Abstractions;

namespace Licenta.DataAccess.Abstractions
{
    public interface IPersistenceContext
    {
        ICustomerRepository CustomerRepository { get; }

        IOrderRepository OrderRepository { get; }
        IRecipientRepository RecipientRepository { get; }
        IDispatcherRepository DispatcherRepository { get; }
        ISupervisorRepository SupervisorRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        IDriverRepository DriverRepository { get; }
        IRouteRepository RouteRepository { get; }

        TransactionScope BeginTransaction();
        void SaveChanges();
    }
}
