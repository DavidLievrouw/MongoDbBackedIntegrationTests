using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DavidLievrouw.Api.Tests
{
    [TestClass]
    public class HomeController : TestsBase
    {
        [TestClass]
        public class UpsertSomeEntries : HomeController
        {
            [TestMethod]
            public async Task CallUpsertSomeEntries()
            {
                var response = await Client.PostAsync(Client.BaseAddress, null);
                Assert.IsTrue(response.IsSuccessStatusCode);
                var json = await response.Content.ReadAsStringAsync();
                Assert.IsNotNull(json);
                var entries = JsonConvert.DeserializeObject<IEnumerable<Api.HomeController.Entry>>(json);
                Assert.IsNotNull(entries);
                Assert.AreEqual(entries.Count(), 3);
            }
        }
    }
}