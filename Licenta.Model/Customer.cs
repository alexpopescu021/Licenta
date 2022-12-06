using System;
using System.Collections.Generic;


namespace Licenta.Model
{
    public class Customer : DataEntity
    {
        public string Name { get; protected set; }
        public virtual Contact ContactDetails { get; protected set; }
        public virtual ICollection<LocationAddress> LocationAddresses { get; protected set; }

        protected Customer()
        { }

        public static Customer Create(string name, string phoneNo, string email)
        {
            var createdCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = name,
                ContactDetails = Contact.Create(phoneNo, email),
                LocationAddresses = new List<LocationAddress>()
            };

            return createdCustomer;
        }

        public void AddLocationAddress(LocationAddress locationAddress)
        {
            LocationAddresses.Add(locationAddress);
        }

        public Contact UpdateContactDetails(string phoneNo, string email)
        {
            var updatedContact = ContactDetails.Update(phoneNo, email);
            return updatedContact;
        }

        public Customer UpdateCustomer(string name, Contact contact)
        {
            Name = name;
            ContactDetails = contact;

            return this;
        }

        public static Customer Create(string id, string name, string phoneNo, string email)
        {
            var createdCustomer = new Customer()
            {
                Id = Guid.Parse(id),
                Name = name,
                ContactDetails = Contact.Create(phoneNo, email),
                LocationAddresses = new List<LocationAddress>()
            };

            return createdCustomer;
        }
    }
}
