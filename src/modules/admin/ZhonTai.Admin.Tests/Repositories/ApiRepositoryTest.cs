using System.Threading.Tasks;
using Xunit;
using ZhonTai.Admin.Domain.Api;

namespace ZhonTai.Admin.Tests.Repositories;

public class ApiRepositoryTest : BaseTest
{
    private readonly IApiRepository _apiRepository;

    public ApiRepositoryTest()
    {
        _apiRepository = GetService<IApiRepository>();
    }

    [Fact]
    public async Task GetAsync()
    {
        var id = 161227167658053;
        var api = await _apiRepository.GetAsync(a => a.Id == id);
        Assert.Equal(id, api?.Id);
    }
}