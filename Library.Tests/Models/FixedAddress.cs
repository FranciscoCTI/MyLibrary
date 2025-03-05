using System;
using System.Collections.Generic;
using System.Linq;
using Library.Core.Interfaces;

namespace Library.Tests.Models
{
    /// <summary>
    /// A special address with sample data, to be used on tests.
    /// </summary>
    internal class FixedAddress : IAddress
    {
        /// <inheritdoc/>
        public string AddressId { get; set; }

        /// <inheritdoc />
        public string Street { get; set; }

        /// <inheritdoc />
        public int HouseNumber { get; set; }

        /// <inheritdoc />
        public string City { get; set; }

        /// <inheritdoc />
        public string Country { get; set; }

        /// <summary>
        /// Constructor for this sample class
        /// </summary>
        public FixedAddress()
        {
            Street = "Martin de Mujica";
            HouseNumber = 255;
            City = "Concepción";
            Country = "Chile";
        }
    }
}
