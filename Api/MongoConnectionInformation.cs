using System;
using MongoDB.Driver;

namespace DavidLievrouw.Api
{
    public class MongoConnectionInformation
    {
        public MongoConnectionInformation(MongoUrl mongoUrl, string databaseName)
        {
            MongoUrl = mongoUrl ?? throw new ArgumentNullException(nameof(mongoUrl));
            DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public MongoUrl MongoUrl { get; }
        public string DatabaseName { get; }
    }
}