using Xunit;
using ZhonTai.Admin.Domain.Api;

namespace ZhonTai.Tests.Repositories;

public class ApiRepositoryTest : BaseTest
{
    private readonly IApiRepository _apiRepository;

    public ApiRepositoryTest()
    {
        _apiRepository = GetService<IApiRepository>();
    }

    [Fact]
    public async void GetAsync()
    {
        var id = 161227167658053;
        var user = await _apiRepository.GetAsync(a => a.Id == id);
        Assert.Equal(id, user?.Id);
    }
}