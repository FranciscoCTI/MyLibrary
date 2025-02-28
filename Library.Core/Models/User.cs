using System;
using System.Collections.Generic;
using System.Linq;
using Library.Core.Interfaces;

namespace Library.Core.Models
{
    /// <summary>
    /// A human person that uses the <see cref="ILibrary"/> 
    /// </summary>
    public class User: IUser 
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string LastName { get; set; }

        /// <inheritdoc />
        public int Age { get; set; }

        /// <inheritdoc />
        public IAddress Address { get; set; }

        /// <summary>
        /// The constructor for <see cref="IUser"/>
        /// </summary>
        /// <param name="name">The first name of this user</param>
        public User(string name)
        {
            this.Name = name;
            this.LastName = "-";
            this.Age = 0;
            this.Address = new Address();
        }

    }
}
