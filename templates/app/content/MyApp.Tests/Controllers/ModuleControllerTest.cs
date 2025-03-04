using System.Threading.Tasks;
using Xunit;
using ZhonTai.Admin.Core.Dto;
using MyApp.Api.Contracts.Services.Module.Input;
using MyApp.Api.Contracts.Services.Module.Output;

namespace MyApp.Tests.Controllers;

/// <summary>
/// 模块Api测试
/// </summary>
public class ModuleControllerTest : BaseControllerTest
{
    public ModuleControllerTest() : base()
    {
    }

    [Fact]
    public async Task Add()
    {
        var input = new ModuleAddInput
        {
            Name = "new-module"
        };

        var res = await PostResult($"/api/__app_code/module/add", input);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Update()
    {
        var output = await GetResult<ResultOutput<ModuleGetOutput>>("/api/__app_code/module/get?id=278518195769413");
        var res = await PutResult($"/api/__app_code/module/update", output.Data);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Get()
    {
        var res = await GetResult<ResultOutput<ModuleGetOutput>>("/api/__app_code/module/get?id=278518195769413");
        Assert.True(res.Success);
    }

    [Fact]
    public async Task GetPage()
    {
        await Login();
        var input = new PageInput<ModuleGetPageInput>
        {
            CurrentPage = 1,
            PageSize = 20,
            Filter = new ModuleGetPageInput
            {
                Name = "module"
            }
        };

        var res = await PostResult($"/api/__app_code/module/get-page", input);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Delete()
    {
        var res = await DeleteResult($"/api/__app_code/module/soft-delete?{ToParams(new { id = 278551714857029 })}");
        Assert.True(res.Success);
    }
}