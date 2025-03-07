using System;
using Library.Core.Enums;
using Library.Core.Interfaces;
using Library.Core.Models;
using Library.Solvers;

namespace Library.Core.Factories
{
    public class LibraryFactory
    {
        public LibraryFactory()
        {

        }

        /// <summary>
        /// Creates a new library
        /// </summary>
        public ILibrary CreateLibrary(string libraryName)
        {
            return new Models.Library(libraryName);
        }

        /// <summary>
        /// Creates a new book
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Book CreateBook(string name)
        {
            Book book = new Book(name);
            book.ISBN = NumberGenerator.GetRandom13DigitNumber();
            book.Description = "Description for this book";
            book.AuthorInformation = new AuthorInformation();
            
            book.Themes.Add(Theme.Architecture);
            book.Themes.Add(Theme.Art);
            book.Themes.Add(Theme.Philosophy);

            return book;
        }
    }
}
