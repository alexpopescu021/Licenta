using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


namespace Licenta.Model
{
    public class LocationAddress : DataEntity
    {
        public string Country { get; protected set; }
        public string City { get; protected set; }
        public string Street { get; protected set; }
        public string StreetNumber { get; protected set; }
        public string PostalCode { get; protected set; }
        public string Tag { get; protected set; }

        protected LocationAddress() { }

        public static LocationAddress Create(string country, string city, string street, string streetNumber, string postalCode, string? tag)
        {
            var createdLocation = new LocationAddress()
            {
                Id = Guid.NewGuid(),
                Country = country,
                City = city,
                Street = street,
                StreetNumber = streetNumber,
                PostalCode = postalCode,
                Tag = tag
            };

            return createdLocation;
        }

        public void SetCity(string city)
        {
            City = city;
        }
        public void SetCountry(string country)
        {
            Country = country;
        }
        public void SetStreet(string street)
        {
            Street = street;
        }
        public void SetStreetNumber(string streetNumber)
        {
            StreetNumber = streetNumber;

        }
        public LocationAddress Update(string country, string city, string street, string streetNumber, string postalCode, string? tag)
        {
            Country = country;
            City = city;
            Street = street;
            StreetNumber = streetNumber;
            PostalCode = postalCode;
            Tag = tag;
            return this;

        }
    }
}
