using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Library.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Core.Models
{
    /// <summary>
    /// Store the information of all the <see cref="IAuthor"/>s involved in the creation
    /// of the content
    /// </summary>
    [BsonIgnoreExtraElements]
    public class AuthorInformation:IAuthorInformation
    {
        /// <inheritdoc/>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; }

        private List<IAuthor> _authors;

        /// <inheritdoc/>
        [BsonElement("Authors")]
        public List<IAuthor> Authors
        {
            get
            {
                return _authors;
            }
            set
            {
                _authors = value;
                OnPropertyChanged(nameof(Authors));
            }
        }

        /// <summary>
        /// Constructor for creating a default <see cref="IAuthorInformation"/> with an
        /// empty list of <see cref="IAuthor"/>s
        /// </summary>
        public AuthorInformation()
        {
            Authors = new List<IAuthor>();
            CreateSampleAuthors();
        }

        /// <summary>
        /// Create a short list of default authors, only for development
        /// </summary>
        public void CreateSampleAuthors()
        {
            IAuthor a1 = new Author("Arturo", "Perez Riverté");
            IAuthor a2 = new Author("Roberto", "Ampuero");
            IAuthor a3 = new Author("Mario", "Vargas Llosa");

            Authors.Add(a1);
            Authors.Add(a2);
            Authors.Add(a3);
        }

        /// <inheritdoc/>
        public void AddNewAuthor(IAuthor author)
        {
            Authors.Add(author);
        }

        /// <inheritdoc/>
        public bool AnyAuthor()
        {
            return Authors.Count > 0;
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
