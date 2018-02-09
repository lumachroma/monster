﻿namespace Sandbox.Models.AddressBook
{
    public class Address
    {
        public Address()
        {
            GeoLocation = new GeoLocation();
        }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public GeoLocation GeoLocation { get; set; }
    }
}