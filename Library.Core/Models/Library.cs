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
        public ObservableCollection<IBook> BookCollection => 
            new ObservableCollection<IBook>(Books.Values);

        /// <summary>
        /// Constructor for the <see cref="ILibrary"/>
        /// </summary>
        /// <param name="libraryName">The name of the new <see cref="ILibrary"/></param>
        public Library(string libraryName)
        {
            this.UsersInformation = new LibraryUsersInformation();
            this.Name = libraryName;

            this.InsertDummyBooks();
        }

        /// <summary>
        /// Create dummy <see cref="IBook"/>s for the development phase only
        /// </summary>
        private void InsertDummyBooks()
        {
            IBook b1 = new Book("Don quijote de la mancha");
            b1.CreateIsbnByDefault();
            IBook b2 = new Book("Capitalism, the unknown ideal");
            b2.CreateIsbnByDefault();
            IBook b3 = new Book("Philosophy who needs it");
            b3.CreateIsbnByDefault();

            this.InsertBook(b1);
            this.InsertBook(b2);
            this.InsertBook(b3);
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
                Books[bookToUpdate.ISBN].Author = bookToUpdate.Author;
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
