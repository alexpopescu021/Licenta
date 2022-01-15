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


        TransactionScope BeginTransaction();
        void SaveChanges();
    }
}
