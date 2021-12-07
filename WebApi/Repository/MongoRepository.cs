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
        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<TDocument> _collection;
        private readonly int sleepTimeInMilliseconds = 20;

        public MongoRepository(IMongoDbSettings settings, IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;

            var database = _mongoClient.GetDatabase(settings.DatabaseName);
            var collection = GetCollectionName(typeof(TDocument));
            if (!_mongoClient.GetDatabase(settings.DatabaseName).ListCollectionNames().ToList().Contains(collection))
            {
                _mongoClient.GetDatabase(settings.DatabaseName).CreateCollection(collection);
            }
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
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
                return Task.Run(() =>
                {
                    var task = _collection.InsertOneAsync(document);
                    System.Threading.Thread.Sleep(sleepTimeInMilliseconds);
                    return task;
                });
            }
            else
            {
                return Task.Run(() =>
                {
                    var task = _collection.InsertOneAsync(session, document);
                    System.Threading.Thread.Sleep(sleepTimeInMilliseconds);
                    return task;
                });
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
            System.Threading.Thread.Sleep(sleepTimeInMilliseconds);
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
