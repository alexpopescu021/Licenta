using Licenta.DataAccess.Abstractions;
using System.Transactions;

namespace Licenta.DataAccess.Repositories
{
    public class EFPersistanceContext : IPersistenceContext
    {
        private readonly ApplicationDbContext dbContext;
        private TransactionScope currentTransactionScope = null;
        public EFPersistanceContext(ApplicationDbContext context)
        {
            dbContext = context;
            VehicleRepository = new EFVehicleRepository(context);
            CustomerRepository = new EFCustomerRepository(context);
            OrderRepository = new EFOrderRepository(context);
            RecipientRepository = new EFRecipientRepository(context);
            RouteRepository = new EFRouteRepository(context);
            DispatcherRepository = new EFDispatcherRepository(context);
            EmployeeRepository = new EFEmployeeRepository(context);
            DriverRepository = new EFDriverRepository(context);
        }


        public ICustomerRepository CustomerRepository { get; private set; }
        public IVehicleRepository VehicleRepository { get; private set; }
        public IOrderRepository OrderRepository { get; private set; }
        public IRecipientRepository RecipientRepository { get; private set; }
        public IDispatcherRepository DispatcherRepository { get; private set; }
        public IDriverRepository DriverRepository { get; private set; }
        public IRouteRepository RouteRepository { get; private set; }
        public IEmployeeRepository EmployeeRepository { get; private set; }
        public TransactionScope BeginTransaction()
        {
            if (currentTransactionScope != null)
            {
                throw new TransactionException("A transaction is already in progress for this context");
            }
            currentTransactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });

            return currentTransactionScope;
        }

        public void Dispose()
        {

            dbContext.Dispose();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
            currentTransactionScope?.Complete();

            currentTransactionScope = null;
        }
    }
}
