using System;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using WebApi.Infrastructure;

namespace WebApi.Entity
{
    [ExcludeFromCodeCoverage]
    [BsonCollection("BookActivity")]
    public class BookActivity : IDocument
    {
        public BookActivity()
        {
            Id = new ObjectId();
        }

        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;

        public string Message { get; set; }
    }
}
