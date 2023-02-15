using Xunit;
using ZhonTai.Admin.Services.Api;
using ZhonTai.Admin.Tools.Captcha;

namespace ZhonTai.Tests.Services;

public class ApiServiceTest : BaseTest
{
    private readonly IApiService _apiService;

    private readonly ICaptchaTool _captchaTool;

    public ApiServiceTest()
    {
        _apiService = GetService<IApiService>();

        _captchaTool = GetService<ICaptchaTool>(); ;
    }

    [Fact]
    public async void SlideJigsawTest()
    {
        var data = await _captchaTool.GetAsync("admin:captcha");
    }

    [Fact]
    public async void GetAsync()
    {
        var res = await _apiService.GetAsync(161227168079941);
        Assert.True(res?.Id > 0);
    }
}