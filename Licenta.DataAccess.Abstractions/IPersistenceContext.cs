using System.Transactions;

namespace Licenta.DataAccess.Abstractions
{
    public interface IPersistenceContext
    {
        ICustomerRepository CustomerRepository { get; }

        IOrderRepository OrderRepository { get; }
        IRecipientRepository RecipientRepository { get; }
        IDispatcherRepository DispatcherRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        IDriverRepository DriverRepository { get; }
        IRouteRepository RouteRepository { get; }

        TransactionScope BeginTransaction();
        void SaveChanges();
    }
}
