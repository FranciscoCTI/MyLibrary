using System;
using System.Collections.Generic;
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
    ///  A human person that participated in the creation of a <see cref="IBook"/> 
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Author: IAuthor
    {
        /// <inheritdoc />
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AuthorId { get; set; }

        /// <inheritdoc />
        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        /// <inheritdoc />
        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("CompleteName")]
        public string CompleteName
        {
            get => FirstName + " " + LastName;
        }

        /// <inheritdoc />
        [BsonElement("Age")]
        public int Age { get; set; }

        /// <inheritdoc />
        [BsonElement("Address")]
        public IAddress Address { get; set; }

        /// <summary>
        /// Constructor for creating an <see cref="Author"/> with first name and lastname
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        /// <inheritdoc/>
        public bool Validate()
        {
            return FirstName != "" && LastName != "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
