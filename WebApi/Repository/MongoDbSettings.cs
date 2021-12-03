using System.Diagnostics.CodeAnalysis;

namespace WebApi.Repository
{
    [ExcludeFromCodeCoverage]
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}