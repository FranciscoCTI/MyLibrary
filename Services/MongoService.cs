using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Library.Core.Enums;
using Library.Core.Interfaces;
using Library.Core.Models;
using Library.Global;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Library.Services
{
    /// <summary>
    /// Handle all the connections with MongoDB
    /// </summary>
    public class MongoService
    {
        private IMongoCollection<Book> _books;
        private IMongoDatabase _dataBase;
        
        /// <summary>
        /// Constructor initialize the process.
        /// </summary>
        public MongoService()
        {
            RegisterSerializer();
            RegisterMapping();
        }

        /// <summary>
        /// Method to create a connection with a mongoDB collection and get all the
        /// elements in a <see cref="IMongoCollection{Object}"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to get</typeparam>
        /// <param name="collection">The name of the collection</param>
        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(MongoConstants.MongoConnectionString);
            var db = client.GetDatabase(MongoConstants.MongoDatabaseName);
            return db.GetCollection<T>(collection);
        }

        /// <summary>
        /// Register a new <see cref="ObjectSerializer"/>> to the <see cref="BsonSerializer"/>>
        /// </summary>
        private void RegisterSerializer()
        {
            var objectSerializer = new ObjectSerializer(ObjectSerializer.AllAllowedTypes);
            BsonSerializer.RegisterSerializer(objectSerializer);
        }

        /// <summary>
        /// Register classes that are going to be serialized in MongoDB
        /// </summary>
        private void RegisterMapping()
        {
            BsonClassMap.RegisterClassMap<AuthorInformation>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Author>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Theme>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }

        /// <summary>
        /// Connects to MongoDB and gets from a certain
        /// <see cref="IMongoCollection{TDocument}"/> all the elements.
        /// </summary>
        /// <param name="collection">The name of the given
        /// <see cref="IMongoCollection{TDocument}"/> in the DB that we want to get</param>
        public async Task<List<Book>> GetBooksAsync(string collection)
        {
            _books = ConnectToMongo<Book>(collection);
            var results = await _books.FindAsync(_ => true);
            return results.ToList();
        }

        /// <summary>
        /// Adds a new <see cref="IBook"/>  to the current
        /// <see cref="IMongoCollection{TDocument}"/>
        /// </summary>
        /// <param name="book">The <see cref="IBook"/> to be inserted on the
        /// <see cref="IMongoCollection{TDocument}"/></param>
        public async Task AddBookAsync(Book book)
        {
            _books = ConnectToMongo<Book>(MongoConstants.MongoBooksCollectionName);
            await _books.InsertOneAsync(book);
        }

        /// <summary>
        /// Removes a <see cref="IBook"/>  from the current
        /// <see cref="IMongoCollection{TDocument}"/>
        /// </summary>
        /// <param name="isbn">The id of the <see cref="IBook"/> to be removed from the
        /// <see cref="IMongoCollection{TDocument}"/></param>
        public async Task RemoveBookAsync(long isbn)
        {
            _books = ConnectToMongo<Book>(MongoConstants.MongoBooksCollectionName);
            await _books.DeleteOneAsync(x => x.ISBN == isbn);
        }

        /// <summary>
        /// Updates a <see cref="IBook"/>  in the current                              
        /// <see cref="IMongoCollection{TDocument}"/>
        /// </summary>
        /// <param name="isbn">The id of the <see cref="IBook"/> to be updated in the
        /// <see cref="IMongoCollection{TDocument}"/> </param>
        /// <param name="book">The <see cref="IBook"/> with the correct values to be
        /// inserted on the <see cref="IMongoCollection{TDocument}"/></param>
        public async Task UpdateBookAsync(long isbn, IBook book)
        {
            _books = ConnectToMongo<Book>(MongoConstants.MongoBooksCollectionName);
            var filter = Builders<Book>.Filter.Eq(b => b.ISBN, isbn);
            await _books.ReplaceOneAsync(filter, (Book)book, 
                                        new ReplaceOptions { IsUpsert = true });
        }
    }
}
