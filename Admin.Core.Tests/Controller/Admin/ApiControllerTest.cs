using Admin.Core.Common.Input;
using Admin.Core.Model.Admin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Admin.Core.Tests.Controller.Admin
{
    public class ApiControllerTest : BaseControllerTest
    {
        public ApiControllerTest() : base()
        {
        }

        private async Task<JObject> GetResult(string apiPath, HttpContent httpContent = null)
        {
            await Login();

            var res = httpContent != null ? await Client.PostAsync(apiPath, httpContent) : await Client.GetAsync(apiPath);
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(content);
        }

        [Fact]
        public async Task Get()
        {
            var jObject = await GetResult("/api/admin/api/get?id=161227167658053");
            Assert.Equal(1, jObject["code"]);
        }

        [Fact]
        public async Task GetList()
        {
            var jObject = await GetResult("/api/admin/api/getlist?key=接口管理");
            Assert.Equal(1, jObject["code"]);
        }

        [Fact]
        public async Task GetPage()
        {
            var input = new PageInput<ApiEntity> 
            { 
                CurrentPage = 1, 
                PageSize = 20, 
                Filter = new ApiEntity 
                { 
                    Label = "接口管理" 
                }
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(input));
            var jObject = await GetResult($"/api/admin/api/getpage", httpContent);
            Assert.Equal(1, jObject["code"]);
        }
    }
}