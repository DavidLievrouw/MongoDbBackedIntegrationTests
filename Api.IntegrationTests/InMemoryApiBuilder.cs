using System;
using System.Web.Http;

namespace DavidLievrouw.Api
{
    public class InMemoryApiBuilder
    {
        private IncludeErrorDetailPolicy _includeErrorDetailPolicy;
        private string _mongoConnectionString;

        public static InMemoryApiBuilder New()
        {
            return new InMemoryApiBuilder();
        }
        
        public InMemoryApiBuilder WithIncludeErrorDetailPolicy(IncludeErrorDetailPolicy policy)
        {
            _includeErrorDetailPolicy = policy;
            return this;
        }
        
        public InMemoryApiBuilder WithMongoConnectionString(string mongoConnectionString)
        {
            _mongoConnectionString = mongoConnectionString;
            return this;
        }
        
        public HttpServer Build()
        {
            if (string.IsNullOrWhiteSpace(_mongoConnectionString)) throw new InvalidOperationException("The mongo connection string is not specified.");

            var appConfig = ConfigLoaderForIntegrationTests.LoadConfig();
            var dependencyResolver = AutofacConfig.Configure(appConfig, _mongoConnectionString);
            var container = dependencyResolver.Build();
            var config = WebApiConfigurator.ConfigureWebApi(container);
            config.IncludeErrorDetailPolicy = _includeErrorDetailPolicy;

            return new HttpServer(config);
        }
    }
}