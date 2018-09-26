using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Driver;

namespace DavidLievrouw.Api
{
    [RoutePrefix("")]
    public class HomeController : ApiController
    {
        private readonly MongoConnectionInformation _mongoConnectionInformation;

        public HomeController(MongoConnectionInformation mongoConnectionInformation)
        {
            _mongoConnectionInformation = mongoConnectionInformation ?? throw new ArgumentNullException(nameof(mongoConnectionInformation));
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetRoot()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("The api is running...", System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain)
            };
        }       
        
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> UpsertSomeEntries()
        {
            var entriesToUpsert = new[]
            {
                new Entry {Id = "entry001", Value = "one"},
                new Entry {Id = "entry002", Value = "two"},
                new Entry {Id = "entry003", Value = "three"}
            };
            var bulkUpserts = entriesToUpsert.Select(entry =>
            {
                var filter = Builders<Entry>.Filter.Where(x => x.Id == entry.Id);
                return new ReplaceOneModel<Entry>(filter, entry) {IsUpsert = true};
            });
            var mongoClientSettings = MongoClientSettings.FromUrl(_mongoConnectionInformation.MongoUrl);
            var client = new MongoClient(mongoClientSettings);
            var databaseForClient = client.GetDatabase(_mongoConnectionInformation.DatabaseName);
            var entriesCollection = databaseForClient.GetCollection<Entry>("entries");
            await entriesCollection.BulkWriteAsync(bulkUpserts);
            var entriesToReturn = await entriesCollection.FindAsync(entry => entry.Value != null);
            return Ok(entriesToReturn);
        }

        public class Entry
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }
    }
}