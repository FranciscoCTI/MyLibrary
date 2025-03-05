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
    [BsonIgnoreExtraElements]
    public class AuthorInformation:IAuthorInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        private List<IAuthor> _authors;

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

        public AuthorInformation()
        {
            Authors = new List<IAuthor>();
        }

        public void CreateSampleAuthors()
        {
            IAuthor a1 = new Author("John", "Adams");
            IAuthor a2 = new Author("John", "Wick");
            IAuthor a3 = new Author("John", "Kazinsky");

            Authors.Add(a1);
            Authors.Add(a2);
            Authors.Add(a3);
        }
        public void AddNewAuthor(IAuthor author)
        {
            Authors.Add(author);
        }
        public void InputAuthors(IEnumerable<IAuthor>? contentAuthors)
        {
            Authors.Clear();

            foreach (var author in contentAuthors)
            {
                if (author.Validate())
                {
                    Authors.Add(author);
                }
            }
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
