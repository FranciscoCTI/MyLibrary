using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using Library.Core.Enums;
using Library.Core.Interfaces;
using Library.Solvers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Core.Models
{
    /// <summary>
    /// Represents an instance of a physical paper book
    /// </summary>
    public class Book : IBook
    {
        /// <summary>
        /// The id ob the mongo object in the collection
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; }

        /// <inheritdoc />
        public long ISBN { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string Description { get; set; }

        /// <inheritdoc />
        [BsonElement("Authors")]
        public IAuthorInformation Authors { get; set; }

        public List<Theme> Themes { get; set; }

        /// <summary>
        /// The constructor for <see cref="IBook"/>
        /// </summary>
        /// <param name="name"></param>
        public Book(string name)
        {
            this.Name = name;
            this.Authors = new AuthorInformation();
            this.Description = "-";
        }

        /// <inheritdoc />
        public void CreateIsbnByDefault()
        {
            ISBN = (int)NumberGenerator.GetRandom13DigitNumber();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
