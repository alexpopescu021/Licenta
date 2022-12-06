using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    public class EfCustomerRepository : EfBaseRepository<Customer>, ICustomerRepository
    {
        public EfCustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        { }

        public IEnumerable<Customer> FindByName(string nameToFind)
        {
            var customersList = DbContext.Customers
                                .Where(customer =>
                                            customer.Name
                                            .ToLower()
                                            .Contains(nameToFind.ToLower()));

            return customersList.AsEnumerable();
        }

        public Customer FindByEmail(string emailToFind)
        {
            var foundCustomer = DbContext.Customers.FirstOrDefault(customer => customer.ContactDetails.Email
                .Contains(emailToFind));

            return foundCustomer;
        }

        public Customer FindByPhoneNo(string phoneNo)
        {
            var foundCustomer = DbContext.Customers.FirstOrDefault(customer => customer.ContactDetails.PhoneNo
                .Contains(phoneNo));

            return foundCustomer;
        }

        public new Customer GetById(Guid customerId)
        {
            return DbContext.Customers
                .Include(c => c.ContactDetails)
                .Include(c => c.LocationAddresses)
                .FirstOrDefault(customer => customer.Id == customerId);
        }

        public new IEnumerable<Customer> GetAll()
        {
            return DbContext.Customers.Include(c => c.ContactDetails)
                                        .Include(c => c.LocationAddresses)
                                        .AsEnumerable();
        }

        public void AddLocationToCustomer(Guid customerId, LocationAddress locationAddress)
        {
            var customer = GetById(customerId);
            DbContext.LocationAddresses.Add(locationAddress);
            customer.AddLocationAddress(locationAddress);
            DbContext.SaveChanges();
        }

        public void AddLocation(LocationAddress locationAddress)
        {
            DbContext.LocationAddresses.Add(locationAddress);
            DbContext.SaveChanges();
        }

        public bool RemoveCustomer(Guid customerId)
        {
            var entityToRemove = GetById(customerId);

            if (entityToRemove == null) return false;
            if (entityToRemove.LocationAddresses.Any())
            {
                foreach (var location in entityToRemove.LocationAddresses)
                {
                    DbContext.Remove(location);
                }
            }

            DbContext.Remove(entityToRemove.ContactDetails);
            DbContext.Remove(entityToRemove);
            DbContext.SaveChanges();

            return true;
        }

        public LocationAddress GetLocationAddress(Guid locationId)
        {
            return DbContext.LocationAddresses
                .FirstOrDefault(c => c.Id == locationId);
        }

        public void RemoveLocation(Guid locationId)
        {
            var address = DbContext.LocationAddresses.FirstOrDefault(a => a.Id == locationId);

            if (address != null) DbContext.Remove((object)address);
            DbContext.SaveChanges();


        }
        public IEnumerable<LocationAddress> GetLocations(Guid customerId)
        {
            throw new NotImplementedException();
        }
    }
}
