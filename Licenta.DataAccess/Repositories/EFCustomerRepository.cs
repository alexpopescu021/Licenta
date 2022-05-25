using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    public class EFCustomerRepository : EFBaseRepository<Customer>, ICustomerRepository
    {
        public EFCustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        { }

        public IEnumerable<Customer> FindByName(string nameToFind)
        {
            var customersList = dbContext.Customers
                                .Where(customer =>
                                            customer.Name
                                            .ToLower()
                                            .Contains(nameToFind.ToLower()));

            return customersList.AsEnumerable();
        }

        public Customer FindByEmail(string emailToFind)
        {
            var foundCustomer = dbContext.Customers
                            .Where(customer =>
                                        customer.ContactDetails.Email
                                        .Contains(emailToFind)).FirstOrDefault();

            return foundCustomer;
        }

        public Customer FindByPhoneNo(string phoneNo)
        {
            var foundCustomer = dbContext.Customers
                            .Where(customer =>
                                    customer.ContactDetails.PhoneNo
                                    .Contains(phoneNo)).FirstOrDefault();

            return foundCustomer;
        }

        public new Customer GetById(Guid customerId)
        {
            return dbContext.Customers.Include(c => c.ContactDetails)
                                        .Include(c => c.LocationAddresses)
                                        .Where(customer => customer.Id == customerId)
                                        .FirstOrDefault();
        }

        public new IEnumerable<Customer> GetAll()
        {
            return dbContext.Customers.Include(c => c.ContactDetails)
                                        .Include(c => c.LocationAddresses)
                                        .AsEnumerable();
        }

        public void AddLocationToCustomer(Guid customerId, LocationAddress locationAddress)
        {
            var customer = GetById(customerId);
            dbContext.LocationAddresses.Add(locationAddress);
            customer.AddLocationAddress(locationAddress);
            dbContext.SaveChanges();
        }

        public void AddLocation(LocationAddress locationAddress)
        {
            dbContext.LocationAddresses.Add(locationAddress);
            dbContext.SaveChanges();
        }

        public bool RemoveCustomer(Guid customerId)
        {
            var entityToRemove = GetById(customerId);

            if (entityToRemove != null)
            {
                if (entityToRemove.LocationAddresses.Count() > 0)
                {
                    foreach (LocationAddress location in entityToRemove.LocationAddresses)
                    {
                        dbContext.Remove(location);
                    }
                }

                dbContext.Remove(entityToRemove.ContactDetails);
                dbContext.Remove(entityToRemove);
                dbContext.SaveChanges();

                return true;
            }
            return false;
        }

        public LocationAddress GetLocationAddress(Guid locationId)
        {
            return dbContext.LocationAddresses
                            .Where(c => c.Id == locationId)
                            .FirstOrDefault();
        }

        public void RemoveLocation(Guid locationId)
        {
            var address = dbContext.LocationAddresses.Where(a => a.Id == locationId).FirstOrDefault();

            dbContext.Remove(address);
            dbContext.SaveChanges();


        }


        public IEnumerable<LocationAddress> GetLocations(Guid customerId)
        {
            throw new NotImplementedException();
        }
    }
}
