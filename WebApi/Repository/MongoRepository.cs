using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Infrastructure;

namespace WebApi.Repository
{
    [ExcludeFromCodeCoverage]
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
        where TDocument : IDocument
    {
        private IMongoClient _mongoClient;
        private readonly MongoClientSettings _mongoClientSettings;
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoDbSettings settings)
        {
            //string connectionString = "mongodb://admin:MyPassword@87.15.22.12:27017/admin?connect=replicaSet&replicaSet=rs0";
            //var mongoUrl = new MongoUrlBuilder(connectionString);
            var mongoUrl = new MongoUrlBuilder(settings.ConnectionString);
            _mongoClientSettings = new MongoClientSettings
            {
                Server = new MongoServerAddress(mongoUrl.Server.Host, mongoUrl.Server.Port),
                Credential = MongoCredential.CreateCredential(mongoUrl.DatabaseName, mongoUrl.Username, mongoUrl.Password)
            };

            _mongoClient = new MongoClient(_mongoClientSettings);
            var database = _mongoClient.GetDatabase(settings.DatabaseName);
            var collection = GetCollectionName(typeof(TDocument));
            //if (!database.ListCollections(new ListCollectionsOptions { Filter = new BsonDocument("name", collection) }).Any())
            //{
            //    database.CreateCollection(collection);
            //}
            if (!_mongoClient.GetDatabase(database.DatabaseNamespace.DatabaseName).ListCollectionNames().ToList().Contains(collection))
            {
                _mongoClient.GetDatabase(database.DatabaseNamespace.DatabaseName).CreateCollection(collection);
            }
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        //public IClientSessionHandle MongoClientStartSession()
        //{
        //    return _mongoClient.StartSession();
        //}

        public IMongoClient MongoClient
        {
            get
            {
                return new MongoClient(_mongoClientSettings);
            }
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual Task<List<TDocument>> FindManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(()=> _collection.Find(filterExpression).ToListAsync());
        }
        
        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }
        
        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual Task InsertOneAsync(IClientSessionHandle session, TDocument document)
        {
            if (session == null)
            {
                return Task.Run(() => _collection.InsertOneAsync(document));
            }
            else
            {
                return Task.Run(() => _collection.InsertOneAsync(session, document));
            }
        }

        public virtual void InsertOne(IClientSessionHandle session, TDocument document)
        {
            if (session == null)
            {
                _collection.InsertOne(document);
            }
            else
            {
                _collection.InsertOneAsync(session, document);
            }
        }

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }
        
        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }
        
        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }
        
        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }
    }
}
