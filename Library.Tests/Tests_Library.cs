using Library.Core;
using Library.Core.Interfaces;
using Library.Core.Models;
using Library.Tests.Models;

namespace Library.Tests
{
    /// <summary>
    /// Tests for the <see cref="ILibrary"/> class
    /// </summary>
    public class LibraryTests
    {
        internal ILibrary Library;

        [SetUp]
        public void Setup()
        {
            Library = new Core.Models.Library("My library");
        }

        #region Demo code
        //[Test]
        //public void Test1()
        //{
        //    //Assert.Throws<ArgumentOutOfRangeException>(something);

        //    //Arrange
        //    //Act
        //    //Assert
        //} 
        #endregion

        [Test]
        public void Library_Name()
        {
            string libraryName = Library.Name;

            Assert.That(libraryName.Equals("My library"));
        }

        [Test]
        public void Library_InsertBook()
        {
            IBook? book = new Book("The brothers Karamazov");
            book.CreateMockIsbn();
            Library.InsertBook(book);

            Assert.That(Library.Books.ContainsKey(book.ISBN));
        }

        [Test]
        public void Library_RemoveBook()
        {
            Book? b = new Book("Un mundo feliz");
            b.CreateMockIsbn();

            Library.InsertBook(b);
            Library.RemoveBook(b.ISBN);

            Assert.That(!Library.Books.Any(x=>
                                            x.Value.Name.Contains("Un mundo feliz")));
            Assert.That(!Library.Books.Keys.Any(x=>x.Equals(b.ISBN)));
        }

        [Test]
        public void Library_AssignOwner()
        {
            IUser user = new User("Francisco");
            user.LastName = "Contreras";
            user.Age = 40;
            user.Address = new FixedAddress();
            Library.AddUser(user);

            int ownerIndex = Library.UsersInformation.Users.IndexOf(user);

            Library.SetOwner(ownerIndex);

            Assert.That(Library.UsersInformation.GetOwner().Address.Street.Contains("ujica"));
        }

        [Test]
        public void Library_AddUser()
        {
            IUser user = new User("Francisco");
            user.LastName = "Contreras";
            user.Age = 40;
            user.Address = new FixedAddress();

            Library.AddUser(user);

            Assert.That(Library.UsersInformation.Users.Any(x=>x.LastName == "Contreras"));
        }
    }
}