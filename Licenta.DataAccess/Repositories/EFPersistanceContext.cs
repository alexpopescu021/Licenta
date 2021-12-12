using Licenta.DataAccess;
using Licenta.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TransportLogistics.DataAccess.Repositories;

namespace Licenta.DataAccess.Repositories
{
    public class EFPersistanceContext : IPersistenceContext
    {
        private readonly ApplicationDbContext dbContext;
        private TransactionScope currentTransactionScope = null;
        public EFPersistanceContext(ApplicationDbContext context)
        {
            this.dbContext = context;

            CustomerRepository = new EFCustomerRepository(context);

          
        }


        public ICustomerRepository CustomerRepository { get; private set; }

        public IOrderRepository OrderRepository { get; private set; }

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
            if (currentTransactionScope != null)
            {
                currentTransactionScope.Complete();
            }

            currentTransactionScope = null;
        }
    }
}
