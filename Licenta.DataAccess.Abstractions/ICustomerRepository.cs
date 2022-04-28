using Licenta.Model;
using System;
using System.Collections.Generic;

namespace Licenta.DataAccess.Abstractions
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        IEnumerable<Customer> FindByName(string nameToFind);
        Customer FindByEmail(string emailToFind);
        Customer FindByPhoneNo(string phoneNo);
        new Customer GetById(Guid customerId);
        new IEnumerable<Customer> GetAll();
        bool RemoveCustomer(Guid customerId);
        void AddLocationToCustomer(Guid customerId, LocationAddress address);
        public void AddLocation(LocationAddress locationAddress);
        void RemoveLocation(Guid locationId);
        IEnumerable<LocationAddress> GetLocations(Guid customerId);
        LocationAddress GetLocationAddress(Guid locationId);
    }
}
