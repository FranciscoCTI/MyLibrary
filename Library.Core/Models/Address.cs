using System;
using System.Collections.Generic;
using System.Linq;
using Library.Core.Interfaces;

namespace Library.Core.Models
{
    /// <summary>
    /// Represents a city address definition
    /// </summary>
    public class Address : IAddress
    {
        /// <inheritdoc />
        public string Street { get; set;}

        /// <inheritdoc />
        public int HouseNumber { get; set; }
        
        /// <inheritdoc />
        public string City { get; set;}

        /// <inheritdoc />
        public string Country { get; set;}

        /// <summary>
        /// The main constructor for the <see cref="IAddress"/> class
        /// </summary>
        public Address()
        {
            Street = "-";
            HouseNumber = -1;
            City = "-";
            Country = "-";
        }
    }
}
