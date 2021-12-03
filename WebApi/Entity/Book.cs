using System;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using WebApi.Infrastructure;

namespace WebApi.Entity
{
    [ExcludeFromCodeCoverage]
    [BsonCollection("Book")]
    public class Book : IDocument
    {
        public Book()
        {
            Id = new ObjectId();
        }

        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;

        public string Title { get; set; }

        public string Author { get; set; }
    }
}
