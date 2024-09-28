using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using ZhonTai.Admin.Services.Api.Dto;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Api.Dto;

namespace ZhonTai.Tests.Controllers;

public class ApiControllerTest : BaseControllerTest
{
    public ApiControllerTest() : base()
    {
    }

    [Fact]
    public async Task Get()
    {
        var res = await GetResult<ResultOutput<ApiGetOutput>>("/api/admin/api/get?id=161227167658053");
        Assert.True(res.Success);
    }

    [Fact]
    public async Task GetList()
    {
        var res = await GetResult<ResultOutput<List<ApiGetListOutput>>>("/api/admin/api/get-list?key=接口管理");
        Assert.True(res.Success);
    }

    [Fact]
    public async Task GetPage()
    {
        await Login();
        var input = new PageInput<ApiGetPageDto>
        {
            CurrentPage = 1,
            PageSize = 20,
            Filter = new ApiGetPageDto
            {
                Label = "接口管理"
            }
        };

        var res = await PostResult($"/api/admin/api/get-page", input);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Add()
    {
        var input = new ApiAddInput
        {
           Label = "新接口",
           Path = "/api/admin/api/newapi",
           HttpMethods = "post"
        };

        var res = await PostResult($"/api/admin/api/add", input);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Update()
    {
        var output = await GetResult<ResultOutput<ApiGetOutput>>("/api/admin/api/get?id=161227167658053");
        var res = await PutResult($"/api/admin/api/update", output.Data);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Delete()
    {
        var res = await DeleteResult($"/api/admin/api/soft-delete?{ToParams(new { id = 191182807191621 })}");
        Assert.True(res.Success);
    }

    //[Fact]
    //public void TestSM()
    //{
    //    //SM2Encryption.GenerateSM2KeyPair(out string privateKey, out string publicKey);
    //    string privateKey = "Ja4UIUJz7XRNDhIiuWXwL78qd1Pc7SC0/Z9LzyF4SL8=";
    //    string publicKey = "BGe1BZDFN+NhCQtc2qlVk8nUlXrIwcyjT3mMt7Xx3BkDNBGBQjFPV0+h3/cGUYXo2TFI1SShS7hWl9zi6SxUHvg=";

    //    string raw = "Hello, SM2 Encryption!";
    //    string e = SM2Encryption.Encrypt(raw, publicKey);
    //    Console.WriteLine($"加密结果：{e}");

    //    string d = SM2Encryption.Decrypt(e, privateKey);
    //    Console.WriteLine($"解密结果：{d}");


    //    string data = "Hello, SM3 Encryption!";
    //    string sm3key = "98C63180C3748ED2";

    //    string sm3Hash = SM3Encryption.ComputeSM3HashBase64(data);
    //    string sm3HashHex = SM3Encryption.ComputeSM3HashHex(data);
    //    //sm3 with key
    //    byte[] hash = SM3Encryption.ComputeHMacSM3(data, sm3key);
    //    string stringHash = Base64.ToBase64String(hash);
    //    string a = SM3Encryption.ComputeSM3HashBase64(data);
    //    string hexHash = Hex.ToHexString(hash); // 如果需要十六进制字符串表示  

    //    Console.WriteLine("Original Data: " + data);
    //    Console.WriteLine("SM3 Hash(string): " + sm3Hash);
    //    Console.WriteLine("SM3 Hash(Hex): " + sm3HashHex);
    //    Console.WriteLine("HMAC-SM3 Hash (string): " + stringHash);
    //    Console.WriteLine("HMAC-SM3 Hash (Hex): " + hexHash);


    //    var msg = "Hello, SM4 Encryption!";
    //    string key = "98C63180C3748ED2";
    //    var iv = "UISwD9fW6cFh9SNS";
    //    var mode = "CBC";
    //    string encryptMsg = SM4Encryption.Encrypt(msg, key, iv, mode, false);
    //    string decryptedMsg = SM4Encryption.Decrypt(encryptMsg, key, iv, mode, false);

    //    //Hex
    //    string encryptMsgHex = SM4Encryption.Encrypt(msg, key, iv, mode, true);
    //    string decryptedMsgHex = SM4Encryption.Decrypt(encryptMsgHex, key, iv, mode, true);
    //    // 打印加密相关信息  

    //    Console.WriteLine("Message:\t\t{0}", encryptMsg);
    //    Console.WriteLine("decryptedMsg:\t\t{0}", decryptedMsg);
    //    Console.WriteLine("MessageHex:\t\t{0}", encryptMsgHex);
    //    Console.WriteLine("decryptedMsgHex:\t{0}", decryptedMsgHex);
    //}

}