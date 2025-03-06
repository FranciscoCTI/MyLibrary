
using System.ComponentModel;

namespace Library.Core.Interfaces
{
    /// <summary>
    /// A human person that have access to the <see cref="ILibrary"/> 
    /// </summary>
    public interface IUser:INotifyPropertyChanged
    {
        /// <summary>
        /// The first name of the <see cref="IUser"/>
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// The last name of the <see cref="IUser"/>
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Returns the complete name of this author
        /// </summary>
        string CompleteName { get; }

        /// <summary>
        /// How old is the <see cref="IUser"/>
        /// </summary>
        int Age { get; set; }

        /// <summary>
        /// Where does the <see cref="IUser"/> lives
        /// </summary>
        IAddress Address { get; set; }
    }
}
