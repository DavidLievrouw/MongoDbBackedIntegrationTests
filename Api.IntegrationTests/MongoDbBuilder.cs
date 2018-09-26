using System;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace DavidLievrouw.Api
{
    public class MongoDbBuilder : IDisposable
    {
        private string _databaseName;
        private MongoDbRunner _runner;

        public static MongoDbBuilder New()
        {
            return new MongoDbBuilder();
        }

        public MongoDbBuilder WithDatabaseName(string databaseName)
        {
            _databaseName = databaseName;
            return this;
        }

        public string Build()
        {
            var pack = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("RecoMaticsConventions", pack, t => true);

            Console.WriteLine($"Starting {nameof(Mongo2Go)}...");
            _runner = MongoDbRunner.Start();
            Console.WriteLine($"Running {nameof(Mongo2Go)} at {_runner.ConnectionString}...");

            return _runner.ConnectionString;
        }

        public void ResetDatabase()
        {
            var client = MongoClient.Create(_runner.ConnectionString);
            var databaseForClient = client.GetDatabase(_databaseName);
            var filter = new FilterDefinitionBuilder<BsonDocument>().And(
                new FilterDefinitionBuilder<BsonDocument>().Ne("name", "configurations"),
                new FilterDefinitionBuilder<BsonDocument>().Ne("name", "rules"));
            var collectionsToDrop = databaseForClient.ListCollections(new ListCollectionsOptions {Filter = filter});
            foreach (var collectionToDrop in collectionsToDrop.ToList())
            {
                var collectionName = collectionToDrop["name"].AsString;
                databaseForClient.DropCollection(collectionName);
            }
        }
        
        public void Dispose()
        {
            if (_runner != null)
            {
                var client = MongoClient.Create(_runner.ConnectionString);
                client.DropDatabase(_databaseName);
                _runner?.Dispose();
            }
        }
    }
}