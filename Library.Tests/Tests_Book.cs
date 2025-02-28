using Library.Core;
using Library.Core.Interfaces;
using Library.Core.Models;

namespace Library.Tests
{
    /// <summary>
    /// Tests for the <see cref="IBook"/>> class
    /// </summary>
    public class BookTests
    {
        internal IBook Book;

        [SetUp]
        public void Setup()
        {
            Book = new Book("Don quijote de la mancha");
            Book.CreateIsbnByDefault();
            Book.Author = "Miguel de servantes y saavedra";
            Book.Description = "El ingenioso hidalgo";
        }


        [Test]
        public void Test1()
        {
 
        }
    }
}