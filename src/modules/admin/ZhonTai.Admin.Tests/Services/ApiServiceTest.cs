using System.Threading.Tasks;
using Xunit;
using ZhonTai.Admin.Services.Api;

namespace ZhonTai.Admin.Tests.Services;

public class ApiServiceTest : BaseTest
{
    private readonly IApiService _apiService;
    public ApiServiceTest()
    {
        _apiService = GetService<IApiService>();
    }

    [Fact]
    public async Task GetAsync()
    {
        var res = await _apiService.GetAsync(161227168079941);
        Assert.True(res?.Id > 0);
    }
}