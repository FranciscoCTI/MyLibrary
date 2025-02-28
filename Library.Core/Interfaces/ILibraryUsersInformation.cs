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
        /// <returns></returns>
        public IUser GetOwner();

        /// <summary>
        /// Sets the index of the <see cref="IUser"/>> that is the owner of the <see cref="ILibrary"/>
        /// </summary>
        /// <param name="newOwner">The index of the new owner</param>
        void SetOwner(int newOwner);
    }
}
