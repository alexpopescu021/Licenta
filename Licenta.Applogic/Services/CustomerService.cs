﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.AppLogic.Services
{
    public class CustomerService
    {
        private readonly IPersistenceContext persistenceContext;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;

        public CustomerService(ICustomerRepository customerRepository, IPersistenceContext persistenceContext, IOrderRepository orderRepository)
        {
            this.persistenceContext = persistenceContext;
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
        }

        public Customer GetCustomerById(string customerId)
        {
            Guid.TryParse(customerId, out var guid);
            var customer = customerRepository.GetById(guid);

            if (customer == null)
            {
                throw new ArgumentNullException();
            }

            return customer;
        }

        public Customer GetCustomerById(Guid customerId)
        {
            return customerRepository.GetById(customerId);
        }

        public IEnumerable<LocationAddress> GetCustomerAddresses(string customerId)
        {

            var customer = GetCustomerById(customerId);

            return customer.LocationAddresses
                            .AsEnumerable();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return customerRepository.GetAll();
        }


        public LocationAddress GetLocationAddress(string locationId)
        {
            Guid.TryParse(locationId, out var locationGuid);
            return customerRepository.GetLocationAddress(locationGuid);
        }

        public Customer CreateNewCustomer(string name, string phoneNo, string email)
        {
            var customer = Customer.Create(name, phoneNo, email);
            customer = customerRepository.Add(customer);
            persistenceContext.SaveChanges();
            return customer;
        }
        public Customer CreateNewCustomerFromIdentity(string id, string name, string phoneNo, string email)
        {
            var customer = Customer.Create(id, name, phoneNo, email);
            customer = customerRepository.Add(customer);
            persistenceContext.SaveChanges();
            return customer;
        }

        public void AddLocationToCustomer(Guid customerId, LocationAddress locationAddress)
        {
            customerRepository.AddLocationToCustomer(customerId, locationAddress);
        }

        public void AddLocation(LocationAddress locationAddress)
        {
            customerRepository.AddLocation(locationAddress);
        }

        public bool RemoveCustomerById(string customerId)
        {
            var customer = GetCustomerById(customerId);

            orderRepository.RemoveOrdersFromCustomer(customer.Id);
            customerRepository.RemoveCustomer(customer.Id);
            persistenceContext.SaveChanges();

            return true;
        }

        public Customer UpdateCustomer(Guid customerId, string name, string phoneNo, string email)
        {
            var customerToUpdate = GetCustomerById(customerId);

            var contact = customerToUpdate.UpdateContactDetails(phoneNo, email);
            customerToUpdate.UpdateCustomer(name, contact);

            persistenceContext.SaveChanges();

            return customerToUpdate;
        }

        public LocationAddress UpdateLocationAddress(Guid locationId, string country, string city,
                                                    string street, string streetNumber, string postalCode, string? tag)
        {
            var locationToUpdate = customerRepository.GetLocationAddress(locationId);

            locationToUpdate.Update(country, city, street, streetNumber, postalCode, tag);
            persistenceContext.SaveChanges();

            return locationToUpdate;
        }

        public bool IsCustomer(string customerId)
        {
            Guid.TryParse(customerId, out var customerGuid);
            if (customerRepository.GetById(customerGuid) != null)
            {
                return true;
            }

            return false;
        }

        public bool IsLocation(string locationId)
        {
            Guid.TryParse(locationId, out var locationGuid);
            if (customerRepository.GetLocationAddress(locationGuid) != null)
            {
                return true;
            }

            return false;
        }

        public void RemoveLocation(string locationId)
        {
            Guid.TryParse(locationId, out var locationGuid);
            customerRepository.RemoveLocation(locationGuid);
        }

    }
}
