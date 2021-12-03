using System;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;

namespace WebApi.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public abstract class Document : IDocument
    {
        protected Document()
        {
            Id = new ObjectId();
        }

        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
