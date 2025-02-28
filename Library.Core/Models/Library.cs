using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Library.Core.Interfaces;

namespace Library.Core.Models
{
    /// <summary>
    /// Represent a location where physical <see cref="IBook"/>s are stored
    /// </summary>
    public class Library : ILibrary
    {
        /// <inheritdoc />
        public IDictionary<long, IBook> Books { get; set; } = 
            new Dictionary<long, IBook>();

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public ILibraryUsersInformation UsersInformation{get;}

        /// <inheritdoc />
        public ObservableCollection<IBook> BookCollection { get; set; }

        /// <summary>
        /// Constructor for the <see cref="ILibrary"/>
        /// </summary>
        /// <param name="libraryName">The name of the new <see cref="ILibrary"/></param>
        public Library(string libraryName)
        {
            this.UsersInformation = new LibraryUsersInformation();
            this.Name = libraryName;
            this.BookCollection = new ObservableCollection<IBook>();
        }

        /// <inheritdoc />
        public void InsertBook(IBook bookToInsert)
        {
            if (bookToInsert.ISBN != 0 && Books.TryAdd(bookToInsert.ISBN, bookToInsert))
            {
                OnPropertyChanged(nameof(BookCollection));
            }
        }

        /// <inheritdoc />
        public void RemoveBook(long bookIsbn)
        {
            if (Books.ContainsKey(bookIsbn))
            {
                Books.Remove(bookIsbn);
                OnPropertyChanged(nameof(BookCollection));
            }
        }

        /// <inheritdoc />
        public void UpdateBook(IBook bookToUpdate)
        {
            if (Books.ContainsKey(bookToUpdate.ISBN))
            {
                Books[bookToUpdate.ISBN].Name = bookToUpdate.Name;
                Books[bookToUpdate.ISBN].Authors = bookToUpdate.Authors;
                Books[bookToUpdate.ISBN].ISBN = bookToUpdate.ISBN;
                Books[bookToUpdate.ISBN].Description = bookToUpdate.Description;

                OnPropertyChanged(nameof(BookCollection));
            }
        }

        /// <inheritdoc />
        public void SumUpBooks()
        {
            foreach (var item in Books)
            {
                IBook book = item.Value;

                Console.WriteLine("Name: {0} | ISBN:{1} " ,book.Name, book.ISBN);
            }
        }

        /// <inheritdoc />
        public void AddUser(IUser user)
        {
            this.UsersInformation.Users.Add(user);
        }

        /// <inheritdoc />
        public void SetOwner(int newOwner)
        {
            this.UsersInformation.SetOwner(newOwner);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? 
                                                 propertyName = null)
        {
            PropertyChanged?.Invoke(this, 
                                    new PropertyChangedEventArgs(propertyName));
        }
    }
}
