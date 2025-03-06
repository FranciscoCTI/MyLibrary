using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using Library.Core.Models;

namespace Library.Core.Interfaces;

/// <summary>
/// Represent a location where physical <see cref="IBook"/>s are stored
/// </summary>
public interface ILibrary:INotifyPropertyChanged
{
    /// <summary>
    /// The whole collection of <see cref="IBook"/>s inside the library.
    /// </summary>
    IDictionary<long, IBook?> Books { get; set; }

    /// <summary>
    /// An <see cref="ObservableCollection{T}"/> of <see cref="IBook"/>s that return all
    /// the elements on the <see cref="ILibrary.Books"/>.
    /// This is used by the <see langword="WPF"/> <see cref="DataGrid"/> to show and
    /// update the content of the library.
    /// </summary>
    public ObservableCollection<IBook> BookCollection { get;}

    /// <summary>
    /// An instance of the class that store the this <see cref="ILibrary"/> users
    /// information, contains the <see cref="ILibraryUsersInformation.Owner"/> and the
    /// list of <see cref="ILibraryUsersInformation.Users"/>.
    /// </summary>
    public ILibraryUsersInformation UsersInformation { get; }

    /// <summary>
    /// The name of this <see cref="ILibrary"/>
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Inserts a new <see cref="IBook"/> in the library 
    /// </summary>
    /// <param name="bookToInsert">The instance of <see cref="IBook"/> to be inserted on
    /// the<see cref="ILibrary"/> collection</param>
    void InsertBook(IBook bookToInsert);

    /// <summary>
    /// Remove a <see cref="IBook"/> from the library
    /// </summary>
    /// <param name="bookIsbn">The isbn code of the <see cref="IBook"/> to remove</param>
    void RemoveBook(long bookIsbn);

    /// <summary>
    /// Updates an existing <see cref="IBook"/> on the <see cref="ILibrary"/>, using the
    /// information from a <see cref="IBook"/> entered by the user
    /// </summary>
    /// <param name="bookToUpdate">An instance of <see cref="IBook"/> with the correct
    /// parameters to replace in the <see cref="ILibrary"/> existing version</param>
    void UpdateBook(IBook bookToUpdate);

    /// <summary>
    /// Prints a lists of the existing <see cref="IBook"/>s on the library to the
    /// <see cref="System.Console"/>
    /// </summary>
    void SumUpBooks();

    /// <summary>
    /// Adds a new user to the <see cref="ILibrary"/>
    /// </summary>
    /// <param name="user">The new <see cref="IUser"/> to be added to the
    /// <see cref="ILibrary"/></param>
    void AddUser(IUser user);

    /// <summary>
    /// Defines the <see cref="ILibraryUsersInformation.Owner"/>
    /// </summary>
    /// <param name="newOwner">The <see cref="IUser"/> to be defined as the owner
    /// of a library in <see cref="ILibraryUsersInformation"/></param>
    void SetOwner(int newOwner);
}