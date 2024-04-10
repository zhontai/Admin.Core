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
        var res = await GetResult<ResultOutput<List<ApiListOutput>>>("/api/admin/api/get-list?key=接口管理");
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
    //public void TestSM2()
    //{

    //    SM2Encrypt.GenerateKeyPair(out string pubk, out string prik);

    //    String plainText = "ererfeiisgod";
    //    byte[] sourceData = Encoding.Default.GetBytes(plainText);

    //    //下面的秘钥可以使用generateKeyPair()生成的秘钥内容  
    //    // 国密规范正式私钥  
    //    String prik1 = "3690655E33D5EA3D9A4AE1A1ADD766FDEA045CDEAA43A9206FB8C430CEFE0D94";
    //    // 国密规范正式公钥  
    //    String pubk2 = "04F6E0C3345AE42B51E06BF50B98834988D54EBC7460FE135A48171BC0629EAE205EEDE253A530608178A98F1E19BB737302813BA39ED3FA3C51639D7A20C7391A";

    //    System.Console.Out.WriteLine("加密: ");
    //    String cipherText = SM2Utils.Encrypt(Hex.Decode(pubk), sourceData);
    //    System.Console.Out.WriteLine(cipherText);
    //    System.Console.Out.WriteLine("解密: ");
    //    plainText = Encoding.Default.GetString(SM2Utils.Decrypt(Hex.Decode(prik), Hex.Decode(cipherText)));
    //    System.Console.Out.WriteLine(plainText);
    //    Assert.Equal("ererfeiisgod", plainText);
    //}

    //[Fact]
    //public void TestSM3()
    //{
    //    var a = SM3Encrypt.Encrypt("sss");
    //    Assert.Equal("a6575581922eb9a386a8b941f2fd1f3ea8ef92051edc3a0dc277a972fc353c14", a);
    //}
    //[Fact]
    //public void TestSM4()
    //{
    //    var a = SM4Encrypt.Encrypt_ECB("1", "c4ca4238a0b923820dcc509a6f75849b");
    //    var b = SM4Encrypt.Decrypt_ECB(a, "c4ca4238a0b923820dcc509a6f75849b");

    //    var a1 = SM4Encrypt.Encrypt_CBC("1", "c4ca4238a0b923820dcc509a6f75849b", "c4ca4238a0b923820dcc509a6f75849b");
    //    var b1 = SM4Encrypt.Decrypt_CBC(a1, "c4ca4238a0b923820dcc509a6f75849b", "c4ca4238a0b923820dcc509a6f75849b");
    //    Assert.Equal("1", b);
    //    Assert.Equal("1", b1);

    //}

}