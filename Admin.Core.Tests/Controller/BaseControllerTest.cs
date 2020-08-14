
using System.Net;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Admin.Core.Service.Admin.Auth.Input;
using Admin.Core.Service.Admin.Auth;
using Admin.Core.Common.Cache;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.Auth.Output;
using Admin.Core.Common.Configs;

namespace Admin.Core.Tests.Controller
{
    public class BaseControllerTest : BaseTest
    {
        private readonly ICache _cache;
        private readonly IAuthService _authService;
        private readonly AppConfig _appConfig;

        protected BaseControllerTest()
        {
            _cache = GetService<ICache>();
            _authService = GetService<IAuthService>();
            _appConfig = GetService<AppConfig>();
        }

        public ByteArrayContent GetHttpContent(object input)
        {
            var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(input));
            var httpContent = new ByteArrayContent(content);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");
            return httpContent;
        }

        public async Task Login(AuthLoginInput input = null)
        {
            if(input == null && _appConfig.VarifyCode.Enable)
            {
                var res = await _authService.GetVerifyCodeAsync("") as IResponseOutput<AuthGetVerifyCodeOutput>;
                var verifyCodeKey = string.Format(CacheKey.VerifyCodeKey, res.Data.Key);
                var verifyCode = await _cache.GetAsync(verifyCodeKey);
                input = new AuthLoginInput() 
                { 
                    UserName = "admin",
                    Password = "111111",
                    VerifyCodeKey = res.Data.Key,
                    VerifyCode = verifyCode
                };
            }

            //Client.DefaultRequestHeaders.Connection.Add("keep-alive");
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
            
            var result = await Client.PostAsync($"/api/admin/auth/login", GetHttpContent(input));
            var content = await result.Content.ReadAsStringAsync();
            var jObject = JsonConvert.DeserializeObject<JObject>(content);

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jObject["data"]["token"]}");
        }

        public string ToParams(object source)
        {
            var stringBuilder = new StringBuilder(string.Empty);
            if (source == null)
            {
                return "";
            }

            var entries = from PropertyDescriptor property in TypeDescriptor.GetProperties(source)
                    let value = property.GetValue(source)
                    where value != null
                    select (property.Name, value);

            foreach (var (name, value) in entries)
            {
                stringBuilder.Append(WebUtility.UrlEncode(name) + "=" + WebUtility.UrlEncode(value + "") + "&");
            }

            return stringBuilder.ToString().Trim('&');
        }
    }
}
