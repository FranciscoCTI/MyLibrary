using System;
using Library.Core.Interfaces;
using Library.Core.Models;

namespace Library.Core.Factories
{
    public class LibraryFactory
    {
        public LibraryFactory()
        {

        }

        public ILibrary CreateLibrary()
        {
            return new Models.Library("The books on my house");
        }

        public IBook? CreateBook(string name)
        {
            return new Book(name);
        }
    }
}
