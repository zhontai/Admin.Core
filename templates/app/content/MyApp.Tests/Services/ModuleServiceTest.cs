using Xunit;
using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using MyApp.Api.Contracts.Services.Module;
using MyApp.Api.Contracts.Services.Module.Input;

namespace MyApp.Tests.Services;

/// <summary>
/// 模块服务测试
/// </summary>
public class ModuleServiceTest : BaseTest
{
    private readonly IModuleService _moduleService;

    public ModuleServiceTest()
    {
        _moduleService = GetService<IModuleService>();
    }

    [Fact]
    public async Task GetPageAsync()
    {
        var input = new PageInput<ModuleGetPageInput>() { };
        var output = await _moduleService.GetPageAsync(input);
        Assert.True(output?.Total >= 0);
    }
}