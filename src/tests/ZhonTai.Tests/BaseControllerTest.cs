using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using ZhonTai.Admin.Tools.Cache;
using ZhonTai.Admin.Tools.Captcha;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Services.Auth.Dto;
using ZhonTai.Admin.Core.Enums;
using System.Collections.Generic;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Tests;

public class BaseControllerTest : BaseTest
{
    private readonly ICacheTool _cache;
    private readonly ICaptchaTool _captcha;
    private readonly AppConfig _appConfig;

    protected BaseControllerTest()
    {
        _cache = GetService<ICacheTool>();
        _captcha = GetService<ICaptchaTool>();
        _appConfig = GetService<AppConfig>();
    }

    public static HttpContent GetHttpContent(object input, string contentType = "application/json;charset=UTF-8", ContentTypeEnum contentTypeEnum = ContentTypeEnum.Json)
    {
        // HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(input));
        var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(input));
        HttpContent httpContent;
        if (contentTypeEnum == ContentTypeEnum.FormData)
        {
            httpContent = new FormUrlEncodedContent(JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(input)));
        }
        else
        {
            httpContent = new ByteArrayContent(content);
        }
        httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
        return httpContent;
    }

    public static HttpContent GetHttpContent(object input, ContentTypeEnum contentTypeEnum)
    {
        var contentType = contentTypeEnum switch
        {
            ContentTypeEnum.Json => "application/json;charset=UTF-8",
            ContentTypeEnum.FormData => "application/x-www-form-urlencoded;charset=UTF-8",
            _ => string.Empty
        };
        return GetHttpContent(input, contentType, contentTypeEnum);
    }

    public async Task<T> GetResult<T>(string apiPath, object input = null, bool checkStatus = true)
    {
        await Login();
        if (input != null)
        {
            var queryParams = ToParams(input);
            apiPath = apiPath.IndexOf('?') > -1 ? $"{apiPath}&{queryParams}" : $"{apiPath}?{queryParams}";
        }
        var res = await Client.GetAsync(apiPath);
        if (checkStatus)
        {
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
        var content = await res.Content.ReadAsStringAsync();
        return content.NotNull() ? JsonConvert.DeserializeObject<T>(content) : default;
    }

    public async Task<T> PostResult<T>(string apiPath, object input = null, bool checkStatus = true, string contentType = "application/json;charset=UTF-8")
    {
        await Login();
        var res = await Client.PostAsync(apiPath, GetHttpContent(input, contentType));
        if (checkStatus)
        {
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
        var content = await res.Content.ReadAsStringAsync();
        return content.NotNull() ? JsonConvert.DeserializeObject<T>(content) : default;
    }

    public async Task<string> PostResultAndGetContent(string apiPath, object input = null, bool checkStatus = true, string contentType = "application/json;charset=UTF-8")
    {
        //application/json;charset=UTF-8
        //application/x-www-form-urlencoded;charset=UTF-8
        await Login();
        var res = await Client.PostAsync(apiPath, GetHttpContent(input, contentType));
        if (checkStatus)
        {
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
        var content = await res.Content.ReadAsStringAsync();
        return content;
    }

    public async Task<T> PutResult<T>(string apiPath, object input = null, bool checkStatus = true, string contentType = "application/json;charset=UTF-8")
    {
        await Login();
        var res = await Client.PutAsync(apiPath, GetHttpContent(input, contentType));
        if (checkStatus)
        {
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
        var content = await res.Content.ReadAsStringAsync();
        return content.NotNull() ? JsonConvert.DeserializeObject<T>(content) : default;
    }

    public async Task<T> DeleteResult<T>(string apiPath, object input = null, bool checkStatus = true)
    {
        await Login();
        if (input != null)
        {
            var queryParams = ToParams(input);
            apiPath = apiPath.IndexOf('?') > -1 ? $"{apiPath}&{queryParams}" : $"{apiPath}?{queryParams}";
        }
        var res = await Client.DeleteAsync(apiPath);
        if (checkStatus)
        {
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
        var content = await res.Content.ReadAsStringAsync();
        return content.NotNull() ? JsonConvert.DeserializeObject<T>(content) : default;
    }

    public async Task<ResultOutput<dynamic>> GetResult(string apiPath, object input = null, bool checkStatus = true)
    {
        return await GetResult<ResultOutput<dynamic>>(apiPath, input, checkStatus);
    }

    public async Task<ResultOutput<dynamic>> PostResult(string apiPath, object input = null, bool checkStatus = true, string contentType = "application/json;charset=UTF-8")
    {
        return await PostResult<ResultOutput<dynamic>>(apiPath, input, checkStatus, contentType);
    }

    public async Task<ResultOutput<dynamic>> PutResult(string apiPath, object input = null, bool checkStatus = true, string contentType = "application/json;charset=UTF-8")
    {
        return await PutResult<ResultOutput<dynamic>>(apiPath, input, checkStatus, contentType);
    }

    public async Task<ResultOutput<dynamic>> DeleteResult(string apiPath, object input = null, bool checkStatus = true)
    {
        return await DeleteResult<ResultOutput<dynamic>>(apiPath, input, checkStatus);
    }

    public async Task Login(AuthLoginInput input = null)
    {
        var authorization = Client.DefaultRequestHeaders.FirstOrDefault(a => a.Key == "Authorization");
        if (authorization.Key != null)
        {
            return;
        }

        if (input == null)
        {
            input = new AuthLoginInput()
            {
                UserName = "admin",
                Password = "111111"
            };
            if (_appConfig.VarifyCode.Enable)
            {
                var res = await _captcha.GetAsync(CacheKeys.Captcha);
                var captchaKey = CacheKeys.Captcha + res.Token;
                var captchaData = await _cache.GetAsync(captchaKey);
                input.Captcha = new CaptchaInput { CaptchaKey = CacheKeys.Captcha, Token = res.Token, Data = JsonConvert.SerializeObject(new { X = captchaData }) };
            }
        }

        //Client.DefaultRequestHeaders.Connection.Add("keep-alive");
        Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");

        var result = await Client.PostAsync($"/api/admin/auth/login", GetHttpContent(input));
        var content = await result.Content.ReadAsStringAsync();
        var jObject = JsonConvert.DeserializeObject<JObject>(content);
        var token = jObject["data"]["token"];

        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    public static string ToParams(object source)
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