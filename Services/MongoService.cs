using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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

            var client = new MongoClient( MongoConstants.MongoConnectionString);
            _dataBase = client.GetDatabase(MongoConstants.MongoDatabaseName);
            GetCurrentCollection(MongoConstants.MongoBooksCollectionName);
        }

        private void RegisterSerializer()
        {
            var objectSerializer = new ObjectSerializer(ObjectSerializer.AllAllowedTypes);
            BsonSerializer.RegisterSerializer(objectSerializer);
        }

        private void GetCurrentCollection(string collection)
        {
            _books = _dataBase.GetCollection<Book>(collection);
        }

        public static void RegisterMapping()
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
        }

        public async Task<IAsyncCursor<Book>> GetBooksAsync()
        {
            GetCurrentCollection("Books");
            return await _books.FindAsync(_ => true);
        }

        public async Task AddBookAsync(Book book)
        {
            await _books.InsertOneAsync(book);
        }

        public async Task RemoveBookAsync(long isbn)
        {
            await _books.DeleteOneAsync(x => x.ISBN == isbn);
        }

        public async Task UpdateBookAsync(long isbn, IBook book)
        {
            var filter = Builders<Book>.Filter.Eq(b => b.ISBN, isbn);

            GetCurrentCollection("Books");

            await _books.ReplaceOneAsync(filter, (Book)book, new ReplaceOptions { IsUpsert = true });
        }
    }
}
