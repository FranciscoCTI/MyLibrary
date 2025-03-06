using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Interfaces
{
    /// <summary>
    /// Store information about the users and Owner in a <see cref="ILibrary"/>
    /// </summary>
    public interface ILibraryUsersInformation:INotifyPropertyChanged
    {
        /// <summary>
        /// The index of the <see cref="IUser"/> that controls the library
        /// </summary>
        int Owner { get; set; }

        /// <summary>
        /// The list of all the <see cref="IUser"/>s that can access the library
        /// </summary>
        List<IUser> Users { get; set; }

        /// <summary>
        /// Get you the owner of the <see cref="ILibrary"/>
        /// </summary>
        public IUser GetOwner();

        /// <summary>
        /// Set the index of the <see cref="IUser"/> that is the main <see cref="IUser"/> of the <see cref="ILibrary"/>
        /// </summary>
        /// <param name="newOwner">The index of the main <see cref="IUser"/>, in the <see cref="Users"/>> List</param>
        void SetOwner(int newOwner);
    }
}
