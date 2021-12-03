using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApi.Infrastructure;

namespace WebApi.Repository
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        IMongoClient MongoClient { get; }

        //IClientSessionHandle MongoClientStartSession();

        IQueryable<TDocument> AsQueryable();

        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);

        Task<List<TDocument>> FindManyAsync(Expression<Func<TDocument, bool>> filterExpression);
        
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindByIdAsync(string id);

        Task InsertOneAsync(IClientSessionHandle session, TDocument document);
        
        void InsertOne(IClientSessionHandle session, TDocument document);

        Task InsertManyAsync(ICollection<TDocument> documents);
        
        Task ReplaceOneAsync(TDocument document);
        
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteByIdAsync(string id);

        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}
