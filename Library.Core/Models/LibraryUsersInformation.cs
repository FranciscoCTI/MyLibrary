using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Core.Interfaces;

namespace Library.Core.Models
{
    /// <summary>
    /// Store information about the users and Owner in a <see cref="ILibrary"/>
    /// </summary>
    internal class LibraryUsersInformation:ILibraryUsersInformation
    {
        /// <inheritdoc />
        public int Owner { get; set; }

        /// <inheritdoc />
        public List<IUser> Users { get; set; }

        /// <inheritdoc />
        public IUser GetOwner()
        {
            return Users[Owner];
        }

        public void SetOwner(int newOwner)
        {
            this.Owner = newOwner;
        }

        /// <summary>
        /// Constructor for <see cref="ILibraryUsersInformation"/>
        /// </summary>
        public LibraryUsersInformation()
        {
            Users = new List<IUser>();
            IUser blankUser = new User("Blank");
            Users.Add(blankUser);

            Owner = 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
