﻿using System;


namespace Licenta.Model
{
    public class Contact : DataEntity
    {
        public string PhoneNo { get; protected set; }
        public string Email { get; protected set; }

        protected Contact() { }

        public static Contact Create(string phoneNo, string email)
        {
            var createdContact = new Contact()
            {
                Id = Guid.NewGuid(),
                PhoneNo = phoneNo,
                Email = email
            };

            return createdContact;
        }

        public Contact Update(string phoneNo, string email)
        {
            PhoneNo = phoneNo;
            Email = email;
            return this;
        }
    }
}
