using System.Runtime.CompilerServices;
using Library.Core.Factories;
using Library.Core.Interfaces;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating a new Library");

            LibraryFactory lf = new LibraryFactory();
            ILibrary library= lf.CreateLibrary();

            Console.WriteLine("The name of this library is {0}",library.Name);

            bool continueInputBooks = true;

            while (continueInputBooks)
            {
                Console.WriteLine("Enter the name of the new Book");
                var theNewBookName = Console.ReadLine();

                if (theNewBookName is null)
                {
                    Console.WriteLine("Enter a valid name");
                    return;
                }

                string realBookName = (string)theNewBookName;

                IBook? newBook = lf.CreateBook(realBookName);

                newBook.CreateIsbnByDefault();

                library.InsertBook(newBook);
                Console.WriteLine("Book: {0} inserted, its ISBN is {1}", newBook.Name, newBook.ISBN);
                
                Console.WriteLine("Do you want to keep inserting books?");
                var answer = Console.ReadLine().ToLower();

                if (answer == "n")
                {
                    continueInputBooks = false;
                }
            }

            library.SumUpBooks();

            Console.WriteLine("Those were the books in the library");

            Console.ReadLine();
        }
    }
}
