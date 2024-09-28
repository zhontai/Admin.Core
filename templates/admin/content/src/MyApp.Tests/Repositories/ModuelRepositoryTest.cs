using Xunit;
using MyApp.Api.Domain.Module;

namespace MyApp.Tests.Repositories;

/// <summary>
/// 模块仓储测试
/// </summary>
public class ModuelRepositoryTest : BaseTest
{
    private readonly IModuleRepository _moduleRepository;

    public ModuelRepositoryTest()
    {
        _moduleRepository = GetService<IModuleRepository>();
    }

    [Fact]
    public async void GetAsync()
    {
        var name = "module";
        var module = await _moduleRepository.Select.DisableGlobalFilter("Tenant")
            .Where(a => a.Name == name).ToOneAsync();
        Assert.Equal(name, module?.Name);

    }
}