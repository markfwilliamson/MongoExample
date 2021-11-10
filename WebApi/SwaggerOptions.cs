using System.Diagnostics.CodeAnalysis;

namespace WebApi
{
    [ExcludeFromCodeCoverage]
    public class SwaggerOptions
    {
        public string JsonRoute { get; set; }

        public string Title { get; set; }

        public string Endpoint { get; set; }

        public string Version { get; set; }
    }
}
