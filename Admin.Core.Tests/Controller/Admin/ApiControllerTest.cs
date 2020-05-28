using Xunit;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Admin.Core.Tests.Controller.Admin
{
    public class ApiControllerTest : BaseControllerTest
    {
        public ApiControllerTest()
        {
        }

        [Fact]
        public async void GetList()
        {
            await Login();

            var res = await Client.GetAsync("/api/admin/api/getlist");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);

            var content = await res.Content.ReadAsStringAsync();
            var jObject = JsonConvert.DeserializeObject<JObject>(content);
            Assert.Equal(1, jObject["code"]);
        }
    }
}
