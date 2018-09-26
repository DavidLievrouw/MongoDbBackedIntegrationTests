using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidLievrouw.Api.Tests
{
    [TestClass]
    public abstract class TestsBase
    {
        protected HttpServer Server;
        protected HttpClient Client;
        protected string UserPrincipalName;

        private static MongoDbBuilder _mongoDbBuilder;
        protected static string MongoConnectionString;

        [AssemblyInitialize]
        public static void OneTimeSetup(TestContext context)
        {
            var appConfig = ConfigLoaderForIntegrationTests.LoadConfig();
            var appSettingsReader = new AppSettingsReader(appConfig);
            var databaseName = appSettingsReader.ReadAppSetting("DatabaseName");
            _mongoDbBuilder = MongoDbBuilder.New()
                .WithDatabaseName(databaseName);
            var mongoServerConnectionString = _mongoDbBuilder.Build();
            MongoConnectionString = mongoServerConnectionString.TrimEnd('/') + '/' + databaseName;
        }
        
        [AssemblyCleanup]
        public static void OneTimeCleanUp()
        {
            _mongoDbBuilder?.Dispose();
            MongoConnectionString = null;
        }

        [TestInitialize]
        public void SetUp()
        {
            UserPrincipalName = "admin@david.com";
            Server = HostApiServer();
            Client = CreateApiClient(Server);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            Client?.Dispose();
            Server?.Dispose();
            _mongoDbBuilder?.ResetDatabase();
        }

        protected virtual HttpServer HostApiServer()
        {
            return InMemoryApiBuilder.New()
                .WithMongoConnectionString(MongoConnectionString)
                .WithIncludeErrorDetailPolicy(IncludeErrorDetailPolicy.Always)
                .Build();
        }

        protected virtual HttpClient CreateApiClient(HttpServer host)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));
            return new HttpClient(host) { BaseAddress = new Uri("http://server", UriKind.Absolute) };
        }
    }
}